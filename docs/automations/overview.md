---
layout: page
title: automation-overview
permalink: /automations/overview
---

# Automations Overview

The automation system that I have built so far consists of a series of Python scripts that do different things.  All of these scripts are contained in a single directory and each of the scripts are for a single automation (e.g. one for Trello stuff).  All of these scripts are included into the main `personal_automations.py` script.  This script acts as the launchpad for all the other automation.  

To run an automation interactivley, you must run `python personal_automations.py <command_to_run>` in a terminal.  This will trigger a specific automation 