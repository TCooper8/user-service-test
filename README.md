# user-service-test

Written in F#.
This project is intended to be a sample project for storing and fetching user information.

# Api

## POST /users

Input (application/json):
  - firstName (required): The first name of the user.
  - lastName (required): The last name of the user.
  - dateOfBirth (optional): The date of the birth of the user in DateTime format.

Output (application/json):
  Returns the id of the user as a Uuid.