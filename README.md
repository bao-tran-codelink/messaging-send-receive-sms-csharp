# Send and Receive SMS C#
<a href="http://dev.bandwidth.com"><img src="https://s3.amazonaws.com/bwdemos/BW-VMP.png"/></a>
</div>

 # Table of Contents

<!-- TOC -->

- [Description](#description)
- [Bandwidth](#bandwidth)
- [Environmental Variables](#environmental-variables)
- [Callback URLs](#callback-urls)
    - [Ngrok](#ngrok)

<!-- /TOC -->

# Description
This repository contains two C# projects: SendReceiveSMS and Server. SendReceiveSMS is used to send text messages from your bandwidth number to your `USER_NUMBER`. 
If your environment variables are configured properly, simply running this project should send a text to the number you provide.
Server is used for handling inbound and outbound webhooks from Bandwidth. In order to use the correct endpoints, you must check the "Use multiple callback URLs" box on the application page in Dashboard. Then in Dashboard, set the INBOUND CALLBACK to `/callbacks/inbound/messaging` and the STATUS CALLBACK to `/callbacks/outbound/messaging`.
The same can be accomplished via the Dashboard API by setting `InboundCallbackUrl` and `OutboundCallbackUrl` respectively.

Inbound callbacks are sent notifying you of a received message on a Bandwidth number, this app prints the phone numbers invloved, as well as the text received.
Outbound callbacks are status updates for messages sent from a Bandwidth number, this app has a dedicated response for each type of status update.

# Bandwidth

In order to use the Bandwidth API users need to set up the appropriate application at the [Bandwidth Dashboard](https://dashboard.bandwidth.com/) and create API tokens.

To create an application log into the [Bandwidth Dashboard](https://dashboard.bandwidth.com/) and navigate to the `Applications` tab.  Fill out the **New Application** form selecting the service (Messaging or Voice) that the application will be used for.  All Bandwidth services require publicly accessible Callback URLs, for more information on how to set one up see [Callback URLs](#callback-urls).

For more information about API credentials see [here](https://dev.bandwidth.com/guides/accountCredentials.html#top)

# Environmental Variables
The sample app uses the below environmental variables.
```sh
BW_ACCOUNT_ID                        # Your Bandwidth Account Id
BW_USERNAME                          # Your Bandwidth API Username
BW_PASSWORD                          # Your Bandwidth API Password
BW_MESSAGING_APPLICATION_ID          # Your Messaging Application Id created in the dashboard
BW_NUMBER                            # The Bandwidth phone number involved with this application
USER_NUMBER                          # The user's phone number involved with this application
```

# Callback URLs

For a detailed introduction to Bandwidth Callbacks see https://dev.bandwidth.com/guides/callbacks/callbacks.html

Below are the callback paths:
* `/callbacks/outbound/messaging`     For Outbound Status Callbacks
* `/callbacks/inbound/messaging`      For Inbound Message Callbacks

## Ngrok

A simple way to set up a local callback URL for testing is to use the free tool [ngrok](https://ngrok.com/).  
After you have downloaded and installed `ngrok` run the following command to open a public tunnel to your port (5001)
```cmd
ngrok http https://localhost:5001
```
You can view your public URL at `http://127.0.0.1:4040` after ngrok is running.  You can also view the status of the tunnel and requests/responses here.
