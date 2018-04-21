module Json

open System.Text
open Newtonsoft.Json

let private enc = Encoding.UTF8

let toString value =
  value
  :> obj
  |> JsonConvert.SerializeObject

let toBytes value =
  value
  |> toString
  |> enc.GetBytes

let fromString<'a> (json:string) =
  try
    json
    |> JsonConvert.DeserializeObject<'a>
    |> Choice1Of2
  with e ->
    Choice2Of2 e

let fromBytes (bytes:byte[]) =
  bytes
  |> enc.GetString
  |> fromString