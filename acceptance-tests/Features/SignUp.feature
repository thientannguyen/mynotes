Feature: Sign Up
    In order to access MyNotes
    As a customer
    I want to sign up

    Background: Background context
        Given No one is logged in
        And MyNotes is opened in browser

    Scenario: I am able to sign up
        Given I am on the login screen
        When I start logging in
        And I click on the signup button
        And I input a random name as username and "Qwer1234" as password
        And I click on create button
        Then I can see welcome text on the screen

