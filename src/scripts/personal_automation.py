import datetime
import sys
import logging

logger = logging.getLogger(__name__)
logging.basicConfig(filename='automation.log', encoding='utf-8', level=logging.INFO, format='%(levelname)s - %(asctime)s %(message)s', datefmt='%m/%d/%Y %I:%M:%S %p')

coffee_spreadsheet_id = '16T_HL153YOK-iOx5XALIXKiSak1bX_pcPLxWawwXqak'

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
                # Import the trello_automation.py script
                import trello_automations as trello_automations

                logger.info('Starting daily chore reset')
                trello_automations.reset_daily_chores()
            case "coffee-insert-charge":
                # import the coffee_automations.py
                import coffee_automations as coffee_automations

                # This function requires one more parameter of the value of the coffee
                if(len(sys.argv) >= 3):
                    amount = sys.argv[2]

                    logger.info('Starting coffee charge')
                    client = coffee_automations.coffee_spreadsheet_setup()

                    date_string = str(datetime.date.today())
                    print(date_string)
                    data = [date_string, amount]

                    coffee_automations.insert_coffee_charge(client, data, coffee_spreadsheet_id, logger)
                else:
                    print('You must provide the amount the coffee costs to insert into the spreadsheet')
            case "coffee-person-payment":
                # Import the trello_automation.py script
                import coffee_automations as coffee_automations

                # When this function runs, there must be two more arguments to the script 
                #  the first one is the person's name, and then second is the amount paid
                if(len(sys.argv) >=4):
                    name = str(sys.argv[2]).lower()
                    amount = sys.argv[3]

                    logger.info('Starting coffee person payment')
                    client = coffee_automations.coffee_spreadsheet_setup()
                    data = [name, amount, str(datetime.date.today())]
                    coffee_automations.insert_person_payment(client, data, coffee_spreadsheet_id, logger)
                else:
                    print('You must provide the name and the amount paid when calling this function')


# call the main function
if __name__ == '__main__': main()