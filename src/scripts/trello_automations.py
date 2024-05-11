import requests
import json
import os

#-------------------------------------------------------------------------------------
# Begin Trello automation functions
#-------------------------------------------------------------------------------------
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

#-------------------------------------------------------------------------------------
# End Trello automation functions
#-------------------------------------------------------------------------------------
