import datetime
import sys
import logging
import pika
import pika.credentials
import json

logger = logging.getLogger(__name__)
logging.basicConfig(filename='automation.log', encoding='utf-8', level=logging.INFO, format='%(levelname)s - %(asctime)s %(message)s', datefmt='%m/%d/%Y %I:%M:%S %p')

coffee_spreadsheet_id = '16T_HL153YOK-iOx5XALIXKiSak1bX_pcPLxWawwXqak'

def main():
    # look at the parameters passed to see if a valid one was passed if none was
    #  passed, display the help menu

    if len(sys.argv) < 2:

        exchange_name = 'automations-exchange'
        virtual_host = 'my_vhos'
        host = 'localhost'
        username = 'user'
        password = 'password' 
        queue_name = 'automations'

        connection = pika.BlockingConnection(
            pika.ConnectionParameters(
                host=host, 
                virtual_host=virtual_host, 
                credentials=pika.credentials.PlainCredentials(
                    username=username, 
                    password=password)))
        
        channel = connection.channel()

        channel.exchange_declare(exchange=exchange_name, exchange_type='direct')

        result = channel.queue_declare(queue=queue_name, exclusive=False)

        channel.queue_bind(exchange=exchange_name, queue=queue_name)

        print(' [*] Waiting for logs.  To exit press CRTL+C')

        def callback(ch, method, properties, body):
            run_action(body)

        channel.basic_consume(
            queue=queue_name, on_message_callback=callback, auto_ack=True)

        channel.start_consuming()
    else:
        command = str(sys.argv[1]).lower()
        run_action(command, False)

def run_action(param, isJson = True):
    print(f' [*] New message {param}')

    if isJson:
        param = param.decode('utf-8').lower() # str(sys.argv[1]).lower()

        data = json.loads(param)
        print(data)
        newparam = data['actionkeyword']
    else:
        newparam = param
    
    match newparam:
        case "trello-reset":
            print(' [*] Resetting Trello Board')
            # Import the trello_automation.py script
            import trello_automations as trello_automations

            logger.info('Starting daily chore reset')
            trello_automations.reset_daily_chores(logger)
            logger.info('Trello reset complete')
            print(' [*] Trello reset complete')
        case "coffee-insert-charge":
            # import the coffee_automations.py
            import coffee_automations as coffee_automations

            # This function requires one more parameter of the value of the coffee
            if(len(sys.argv) >= 3):
                amount = sys.argv[2]
            else:
                # get the list of parameters from the data and loop through it to find the right one
                #  in this case we need one called 'amount'
                action_params = data['parameters']

                for item in action_params:
                    if item['name'].lower() == 'amount':
                        amount = item['value']
                        break

            logger.info('Starting coffee charge')
            client = coffee_automations.coffee_spreadsheet_setup()

            date_string = str(datetime.date.today())
            print(date_string)
            data = [date_string, amount]

            coffee_automations.insert_coffee_charge(client, data, coffee_spreadsheet_id, logger)
            #else:
                #print('You must provide the amount the coffee costs to insert into the spreadsheet')
        case "coffee-person-payment":
            # Import the trello_automation.py script
            import coffee_automations as coffee_automations

            # When this function runs, there must be two more arguments to the script 
            #  the first one is the person's name, and then second is the amount paid
            if(len(sys.argv) >=4):
                name = str(sys.argv[2]).lower()
                amount = sys.argv[3]
            else:
                # get the list of parameters from the data and loop through it to find the right one
                #  in this case we need one called 'amount' and one called 'name'
                action_params = data['parameters']

                for item in action_params:
                    if item['name'].lower() == 'amount':
                        amount = item['value']
                    elif item['name'].lower() == 'name':
                        name = item['value']

                    if 'name' in locals() and 'amount' in locals():
                        break
                        

            logger.info('Starting coffee person payment')
            client = coffee_automations.coffee_spreadsheet_setup()
            data = [name, amount, str(datetime.date.today())]
            coffee_automations.insert_person_payment(client, data, coffee_spreadsheet_id, logger)
            #else:
                #print('You must provide the name and the amount paid when calling this function')
        case _:
            print('Unknown Action')
        


# call the main function
if __name__ == '__main__': main()