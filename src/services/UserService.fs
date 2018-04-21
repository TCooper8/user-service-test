module UserService

open System

open TCooper8.FSharp.Sql
open FSharp.AsyncUtil
open UserModel
open Results

let private postQuery = """
  insert into users (
    first_name,
    last_name,
    date_of_birth
  )
  values (
    :first_name,
    :last_name,
    :date_of_birth
  )
  returning id;
"""

let private getQuery = """
  select
    id,
    first_name,
    last_name,
    date_of_birth
  from users
  where id = :id
"""

let private read reader: UserInfo Async =
  [ "id"
    "first_name"
    "last_name"
    "date_of_birth"
  ]
  |> SqlReader.record reader
  |> Async.map (fun fields ->
    { id = Option.get fields.[0] :?> Guid
      firstName = Option.get fields.[1] :?> string
      lastName = Option.get fields.[2] :?> string
      dateOfBirth = Option.get fields.[3] :?> DateTime
    }
  )

let private post db (input:UserInput) =
  printfn "Inserting user %A" input
  SqlQuery.SqlPrepared(
    postQuery,
    [ "first_name", input.firstName :> obj
      "last_name", input.lastName :> obj
      "date_of_birth", input.dateOfBirth :> obj
    ]
  )
  |> fun cmd ->
    cmd
  |> SqlQuery.asScalar db
  |> Async.map Successful.created

let private get db (userId:Guid) =
  SqlQuery.SqlPrepared(
    getQuery,
    [ "id", userId :> obj
    ]
  )
  |> SqlQuery.asReader db
  |> Async.bind (fun reader -> async {
    use reader = reader
    let! reader = SqlReader.read reader
    match reader with
    | None ->
      return RequestErrors.notFound "User does not exist"
    | Some reader ->
      let! record = read reader
      return Successful.ok record
  })

type private Service (db) =
  interface IUserService with
    member __.Post input = post db input
    member __.Get userId = get db userId

let init db =
  Service db
  :> IUserService