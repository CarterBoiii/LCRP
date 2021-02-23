# Launcher
Step 1
Place the API Folder into your website's root directory

Step 2
Edit the connect.php file to make the connection and link with your database

Step 3 
Next upload the sql file into your database

All API is done.

Step 4

Change all the webhook information located in the Login.cs and Register.cs

```
Example:
        string WHusername = "Launcher Logs";
        string WHavatar = "https://cdn.discordapp.com/icons/812094445645856789/76a8914ad96416b5c35e689ce06d84fb.png?size=128";
        string WHurl = "https://discord.com/api/webhooks/813839676447785040/jak0gadZjDg86hAdsjJ0fP8P7tamFNJ4JLauB3KI2HU-mLhE98qhEhezEF8-QKhKVRGt";
```

Step 5 

Create a new build using visual studio.

Step 6 

Run the launcher and create a user and then your all set

Make sure your user has been created and stored in your database

Change USERNAME and PASSWORD only and press enter it's will give you the success page
Now in your database you should see your newly created user inside the table "accounts"

Step 5

You may now start editing the launcher to your liking by opening the folder in visual studios,
next open login.cs with visual studio C# editor and change !

When it's done, build the project and try to login with account you have made.

The user managment runs off the database:
To ban a users account, change Status to "BANNED"
To reset the HWID assoiciated with the account, change whitelist to "RESET"
To reset the accounts ip lock, Change IP to "RESET"

This launcher was in development for a server called Lucid City RP, but it seems they have lost interest in me as there developer so i decided to publicly release this