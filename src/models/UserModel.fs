module UserModel

open System
open Results

type UserId = Guid
type UserInfo = {
  id: Guid
  firstName: string
  lastName: string
  dateOfBirth: DateTime
}

type UserInput = {
  firstName: string
  lastName: string
  dateOfBirth: DateTime
}

[<Interface>]
type IUserService =
  abstract Post: UserInput -> UserId Result Async
  abstract Get: UserId -> UserInfo Result Async

module UserService =
  let internal map (service:IUserService) mapping =
    mapping service

  let post service input =
    map service (fun s -> s.Post input)

  let get service id =
    map service (fun s -> s.Get id)