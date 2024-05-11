import os
import gspread
from oauth2client.service_account import ServiceAccountCredentials

def coffee_spreadsheet_setup():
    path_to_keyfile = os.environ['GOOGLE_SHEETS_KEY_PATH']

    scope = ["https://spreadsheets.google.com/feeds", "https://www.googleapis.com/auth/spreadsheets", "https://www.googleapis.com/auth/drive.file", "https://www.googleapis.com/auth/drive"]

    creds = ServiceAccountCredentials.from_json_keyfile_name(path_to_keyfile, scope)
    client = gspread.authorize(creds)

    return client

def insert_coffee_charge(client, data, doc_key, logger):
    logger.info(f'Appending coffee charge to the spreadsheet: {data}')

    sheet = client.open_by_key(doc_key).get_worksheet(0)

    sheet.append_row(data, value_input_option='USER_ENTERED', table_range='A1:B2')

    logger.info("Update successful")

def insert_person_payment(client, data, doc_key, logger):
    logger.info(f'Appending person payment to the spredsheet: {data}')

    sheet = client.open_by_key(doc_key).get_worksheet(1)

    sheet.append_row(data, value_input_option='USER_ENTERED', table_range='A1:C1')

    logger.info('Update successful!')

# TODO:
# Add functionality to read the current balance and when it is at or near negative
#  Push alert with email text to send?
# Create main function that will update the automation help display text (text file?)
#  and any other configuration stuff maybe?