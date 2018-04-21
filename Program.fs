open System
open System.Net

open Suave
open TCooper8.FSharp.Sql

[<EntryPoint>]
let main _ =
  let dbUrl =
    let url = Env.get "DATABASE_URL" |> Uri
    "Database=postgres; Username=postgres; Password=postgres; Host=localhost"
    
  let port =
    match Env.tryGet "PORT" with
    | None -> 8080us
    | Some port -> UInt16.Parse port

  let userService =
    UserService.init dbUrl

  let app =
    choose
      [ UserController.app userService
      ]

  startWebServer
    { defaultConfig with
        bindings =
          [ HttpBinding.create HTTP IPAddress.Any port
          ]
    }
    app
  0