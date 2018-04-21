module UserController

open System.Text

open Suave
open Suave.Filters
open Suave.Operators

open FSharp.AsyncUtil

open Results
open UserModel

let private enc = Encoding.UTF8

let private statusToCode status: HttpCode =
  match HttpCode.tryParse status with
  | Choice1Of2 code -> code
  | Choice2Of2 _ -> HTTP_500

let writeResult (http:HttpContext) (result:'a Result) =
  let body =
    match result.body with
    | None -> enc.GetBytes result.message
    | Some value -> Json.toBytes value
  let code = statusToCode result.status

  Suave.Response.response code body http

let private post service: WebPart =
  fun http ->
    match Json.fromBytes http.request.rawForm with
      | Choice2Of2 e ->
        async { return ServerErrors.internalError e.Message }
      | Choice1Of2 input ->
        UserService.post service input
    |> Async.bind (writeResult http)

let app userService =
  choose
    [ POST >=> path "/users" >=> post userService
    ]