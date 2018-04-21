module Env

open System

let get key =
  let value = Environment.GetEnvironmentVariable key
  if String.IsNullOrEmpty value then
    failwithf "Expected environment variable `%s` to be defined." key
  else
    value

let tryGet key =
  let value = Environment.GetEnvironmentVariable key
  if String.IsNullOrEmpty value then
    None
  else Some value
