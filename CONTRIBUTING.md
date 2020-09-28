/* CONTRIBUTE */

This is the contribute.md of our project. Great to have you here. Here are a few ways you can help make this project better!

# Contribute.md

## Team members

@cyberknet - Scott Blomfield

## Learn & listen

We welcome pull requests, as well as user feature requests. Feel free to join in the conversation
 * on IRC - frenode/##llectronics
 * on Discord - https://discord.gg/ZZN2yz

## Adding new features

Inversion of control and separation of concerns are at the core of Llectrobot. The code should be
easy for a newcomer to dive into, and also to add features to.

###Pull Requests
* Create an issue, and let us know your intent to work on it.
* Check out if there is a compatible project already created in the area you want to add a feature.
* Create a pull request for the feature, and it will be reviewed/merged.
* Don’t get discouraged! We usually answer promptly, but if you don't get a response to your pull request,
  drop in on one of our communication channels and we'll be glad to give you some real-time feedback.
###Coding Style
* Generally speaking, we follow the conventions as listed out in [Microsoft's documentation](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)

# Bug triage

First and foremost, thanks for thinking of writing a bug report. An even bigger thank you for looking
here to find out how. Before you do, we have a couple of small requests.
* Please make your title a simple, short decription of the problem. We will probably refer to this 
  later by title so make it include relevant information.
    * Perfect: Using command !seen with parameter @username in guild Llectronics provides an incorrect last activity date
    * Needs Work: Command !seen is not working parameter @username 
    * Not Helpful: !seen is broken
* Providing us with reliable steps to reproduce a bug is often crucial for being able to get it
  resolved. Each step should be a distinct action taken and make it easy for someone to follow
  through the program. Prior to submitting a bug, you should have followed your own steps and
  been able to see your bug occur - this will let you see any step/input/activity that was missed
  when you wrote your original documentation.
    * Perfect:
      * Open Discord [version]
      * Select Server [Name]
      * Select Channel [Name]
      * enter command "!seen @username"
    * Needs Work:
      * enter command "!seen @username"
    * Not Helpful
      * request the bot when a user was last seen
* Let us know what you think should have happened.
    * Perfect: The bot received the command, and shows the last activity date for @username in guild
      Llectronics. The last activity date is the last date/time a user spoke on, emoted on, joined,
      or left a channel.
    * Needs Work: The bot shows the last activity date for a user.
    * Not Helpful: The bot shows the correct information.
* Tell us what behavior you observed instead of what should have happened.
    * Perfect: The bot indicated that @username had never spoken in guild LLectronics.
    * Needs Work: The bot tells me @username never spoke.
    * Not Helpful: It was wrong.
* If you can, provide a screenshot of the issue occurring.
    * Perfect: Shows your discord window and the system time. The guild and channel are clearly visible
      and the entire interaction between you and the bot demonstrating the issue. Servers unrelated
      to the issue are blurred or otherwise redacted.
    * Needs Work: Shows your discord window. The guild and channel are visible. Many servers are visible.
      Only the last step of the interaction is visible.
    * Not Helpful: A cropped section of the discord window was captured, showing the last step of the interaction.

* You can help report bugs or search existing bugs [here](https://github.com/cyberknet/LlectroBot/issues)
* You can close fixed bugs by testing old tickets to see if they are still happening.

# Translations

LlectroBot is not set up for multiple languages. This is a future state goal for us.

# Documentation
* Help us with documentation about how to invite LlectroBot to a guild/server in [README.md](https://github.com/cyberknet/LlectroBot/blob/master/README.md)

# Community 
* You can help us answer questions our users have on Discord 
* You can help write blog posts about the project
