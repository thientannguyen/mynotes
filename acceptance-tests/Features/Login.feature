Feature: Login into MyNotes
    In order to access MyNotes
    As a customer
    I want to login

    Background: Background context
        Given No one is logged in
        And MyNotes is opened in browser

    Scenario: Account login
        Given I am on the login screen
        And I start logging in
        When I log in using account "thientan@gmail.com" and password "Qwer1234"
        Then I am succesfully logged in