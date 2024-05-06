import datetime
import os
import requests
import json
import sys
import logging
import gspread
from oauth2client.service_account import ServiceAccountCredentials

logger = logging.getLogger(__name__)
logging.basicConfig(filename='automation.log', encoding='utf-8', level=logging.INFO, format='%(levelname)s - %(asctime)s %(message)s', datefmt='%m/%d/%Y %I:%M:%S %p')

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

#-------------------------------------------------------------------------------------
# Begin Coffee Spreadsheet Automation Functions
#-------------------------------------------------------------------------------------

def coffee_spreadsheet_setup():
    path_to_keyfile = os.environ['GOOGLE_SHEETS_KEY_PATH']

    scope = ["https://spreadsheets.google.com/feeds", "https://www.googleapis.com/auth/spreadsheets", "https://www.googleapis.com/auth/drive.file", "https://www.googleapis.com/auth/drive"]

    creds = ServiceAccountCredentials.from_json_keyfile_name(path_to_keyfile, scope)
    client = gspread.authorize(creds)

    return client

def insert_coffee_charge(client, data, doc_key):
    logger.info(f'Appending coffee charge to the spreadsheet: {data}')

    sheet = client.open_by_key(doc_key).get_worksheet(0)

    sheet.append_row(data)

    logger.info("Update successful")

def insert_person_payment(client, data, doc_key):
    logger.info(f'Appending person payment to the spredsheet: {data}')

    sheet = client.open_by_key(doc_key).get_worksheet(1)

    sheet.append_row(data)

    logger.info('Update successful!')

#-------------------------------------------------------------------------------------
# End Coffee Spreadsheet Automation Functions
#-------------------------------------------------------------------------------------

def display_help():
    print("")
    print("              Personal Automation Script    ")
    print("---------------------------------------------------")
    print("Expected format: reset_trello_chores.py <arguments>")
    print("")
    print("   Possible arguments: ")
    print("      trello-reset")
    print("      coffee-insert-charge")
    print("      coffee-person-payment")
    print("")

def main():
    # look at the parameters passed to see if a valid one was passed if none was
    #  passed, display the help menu

    if len(sys.argv) < 2:
        display_help()
    else:
        param = str(sys.argv[1]).lower()

        match param:
            case "trello-reset":
                logger.info('Starting daily chore reset')
                reset_daily_chores()
            case "coffee-insert-charge":
                # This function requires one more parameter of the value of the coffee
                if(len(sys.argv) >= 3):
                    amount = sys.argv[2]

                    logger.info('Starting coffee charge')
                    client = coffee_spreadsheet_setup()
                    data = [str(datetime.date.today()), amount]
                    insert_coffee_charge(client, data, '1pQblkNchcFNn1MSDa192jps62etW2NW4xUYDjb9VU5A')
                else:
                    print('You must provide the amount the coffee costs to insert into the spreadsheet')
            case "coffee-person-payment":
                # When this function runs, there must be two more arguments to the script 
                #  the first one is the person's name, and then second is the amount paid
                if(len(sys.argv) >=4):
                    name = str(sys.argv[2]).lower()
                    amount = sys.argv[3]

                    logger.info('Starting coffee person payment')
                    client = coffee_spreadsheet_setup()
                    data = [name, str(datetime.date.today()), amount]
                    insert_person_payment(client, data, '1pQblkNchcFNn1MSDa192jps62etW2NW4xUYDjb9VU5A')
                else:
                    print('You must provide the name and the amount paid when calling this function')


# call the main function
if __name__ == '__main__': main()