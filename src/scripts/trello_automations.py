import requests
import json
import os

logger_heading = 'trello_automation: '

# Updates all the checklist items on a card as incomplete
def mark_checklist_incomplete(api_key, api_token, card_id, check_item_id, logger):
    incomplete_url = f"https://api.trello.com/1/cards/{card_id}/checkItem/{check_item_id}"

    data = {
        'key': f'{api_key}',
        'token': f'{api_token}',
        'state': 'incomplete'
    }

    logger.debug(f"{logger_heading}Sending PUT request for checklist item")

    requests.put(incomplete_url, data=data)

# makes sure the provided card is in the right list on the board
def move_card_to_list(api_key, api_token, card_id, list_id, logger):
    update_card_url = f'https://api.trello.com/1/cards/{card_id}'

    data = {
        'key': f'{api_key}',
        'token': f'{api_token}',        
        'idList':f'{list_id}'
    }

    logger.debug(f'{logger_heading}Sending PUT request for card')

    requests.put(update_card_url, data=data)

def reset_daily_chores(logger):
    logger.info(f"{logger_heading}Resetting Trello board...")

    # Get the Trello api_key and api_token from the enviornment variable
    api_key = os.environ['TRELLO_API_KEY']
    api_token = os.environ['TRELLO_API_TOKEN']

    # The 'todo' list id in the chores board
    list_id = os.environ['TRELLO_LIST_ID']

    # The 'chores' board ID
    board_id = os.environ['TRELLO_BOARD_ID']    

    logger.info(f'{logger_heading}Getting all the cards in Trello board')

    # Get all the cards on this board 
    url =  f'https://api.trello.com/1/boards/{board_id}/cards?key={api_key}&token={api_token}'

    response = requests.get(url)
    cards = json.loads(response.content)

    logger.info(f'{logger_heading}Updating all the daily cards')

    # For each card in the board, if it has the label named 'Daily' make sure all the checklist
    #  items are unchecked and move it to the todo list
    for card in cards:
        for label in card['labels']:
            if label['name'] == 'Daily':

                logger.info(f'{logger_heading}Moving card back to todo: {card['name']}')

                move_card_to_list(api_key, api_token, card['id'], list_id, logger)

                for check_item in card['checkItemStates']:
                    
                    logger.info(f'{logger_heading}Clearing checklist item')

                    mark_checklist_incomplete(api_key, api_token, card['id'], check_item['idCheckItem'], logger)
