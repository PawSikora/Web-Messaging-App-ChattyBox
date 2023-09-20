# Web-Messaging-App-ChattyBox

Web application built using .NET and Angular for creating chat groups and sending text messages or files such as images, videos.

## Technologies
* .NET 7.0 WebAPI
* Entity Framework Core 7.0
* xUnit
* Angular 15
* SQL Server Express LocalDB

## Features
* Create and manage group chats, by adding or removing members.
* Send text or file based messages. Support for popular types such as .jpg, .mp4 or .gif

## Overview
Once you open the site you must first create or log intou existing account. You are also notified about cookie use. Inputs for both email and password are validated using regex. Additionally database checks if email is already taken.

![ekranLogowania](https://github.com/PawSikora/Web-Messaging-App-ChattyBox/assets/72358883/ef067085-4947-4e23-b314-7e50a85f2570)


![ekranRejestracji](https://github.com/PawSikora/Web-Messaging-App-ChattyBox/assets/72358883/83ceacd8-c248-432a-bc2d-cfafdb09c608)


After successfully loggin in you are greeted by account's main page with information such as your email, last log in time, account creation date and number of chats you are participating in.

![glownaStronaUzytkownika](https://github.com/PawSikora/Web-Messaging-App-ChattyBox/assets/72358883/9c721c29-4024-4f78-98a1-a7d7388a51e9)


After clicking "Chaty" you are redirected to chat browser which contains a list of all chats you are a part of. Furthermore there is a button that allows for creating new chat.

![listaChatow](https://github.com/PawSikora/Web-Messaging-App-ChattyBox/assets/72358883/afa3d8db-6c7a-44c7-add7-11bfaa1891e2)


For each chat you created, you have the option to add and delete users.

![dodawanieUzytkownikow](https://github.com/PawSikora/Web-Messaging-App-ChattyBox/assets/72358883/aeb460ad-446a-4501-8e49-99324816ab48)

After clicking on the chat you wish to visit, you will be redirected to the chat's dedicated page where you can send messages and, depending on whether you are an administrator, you can also delete messages and the chat itself.


![stronaChatu](https://github.com/PawSikora/Web-Messaging-App-ChattyBox/assets/72358883/8f2d5521-9b3d-4984-8d2a-41cc5abb7663)


## Database diagram

![baza_danychFinalna](https://github.com/PawSikora/Web-Messaging-App-ChattyBox/assets/72358883/b16085d6-cad2-427a-9171-a0073e8cec04)

