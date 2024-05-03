import os
import requests
import json
import sys
import logging

logger = logging.getLogger(__name__)
logging.basicConfig(filename='automation.log', encoding='utf-8', level=logging.INFO, format='%(levelname)s - %(asctime)s %(message)s', datefmt='%m/%d/%Y %I:%M:%S %p')

# Updates all the checklist items on a card as incomplete
def mark_checklist_incomplete(api_key, api_token, card_id, check_item_id):
    incomplete_url = f"https://api.trello.com/1/cards/{card_id}/checkItem/{check_item_id}"

    data = {
        'key': f'{api_key}',
        'token': f'{api_token}',
        'state': 'incomplete'
    }

    requests.put(incomplete_url, data=data)

# makes sure the provided card is in the right list on the board
def move_card_to_list(api_key, api_token, card_id, list_id):
    update_card_url = f'https://api.trello.com/1/cards/{card_id}'

    data = {
        'key': f'{api_key}',
        'token': f'{api_token}',        
        'idList':f'{list_id}'
    }

    requests.put(update_card_url, data=data)

def reset_daily_chores():
    print("Resetting Trello board...")

    # Get the Trello api_key and api_token from the enviornment variable
    api_key = os.environ['TRELLO_API_KEY']
    api_token = os.environ['TRELLO_API_TOKEN']

    # The 'todo' list id in the chores board
    list_id = os.environ['TRELLO_LIST_ID']

    # The 'chores' board ID
    board_id = os.environ['TRELLO_BOARD_ID']    

    # Get all the cards on this board 
    url =  f'https://api.trello.com/1/boards/{board_id}/cards?key={api_key}&token={api_token}'

    response = requests.get(url)
    cards = json.loads(response.content)

    # For each card in the board, if it has the label named 'Daily' make sure all the checklist
    #  items are unchecked and move it to the todo list
    for card in cards:
        for label in card['labels']:
            if label['name'] == 'Daily':
                move_card_to_list(api_key, api_token, card['id'], list_id)

                for check_item in card['checkItemStates']:
                    mark_checklist_incomplete(api_key, api_token, card['id'], check_item['idCheckItem'])

def display_help():
    print("")
    print("              Personal Automation Script    ")
    print("---------------------------------------------------")
    print("Expected format: reset_trello_chores.py <arguments>")
    print("")
    print("   Possible arguments: trello-reset")
    print("")

def main():
    # look at the parameters passed to see if a valid one was passed if none was
    #  passed, display the help menu

    if len(sys.argv) != 2:
        display_help()
    else:
        param = str(sys.argv[1]).lower()

        match param:
            case "trello-reset":
                logger.info('Starting daily chore reset')
                reset_daily_chores()

# call the main function
if __name__ == '__main__': main()