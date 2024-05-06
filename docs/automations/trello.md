---
layout: page
title: Trello Automation
permalink: /about/
---

## Requirements

The following items are required to run this automation, a Trello API key, a Trello API token, the board ID that you wish to automate, and the ID of the list to move the cards to.  These values are stored as environment variables on the system as follows:

API Key = TRELLO_API_KEY
API Token = TRELLO_API_TOKEN
List ID = TRELLO_LIST_ID
Board ID = TRELLO_BOARD_ID

## About

This automation will move all cards on a board with a label named 'Daily' on them back to the 'TODO' list and reset all checklist items on each card back to unchecked.