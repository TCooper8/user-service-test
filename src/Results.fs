module Results

module Codes =
  let ok = 200
  let created = 201
  let accepted = 202
  let noContent = 204
  let badRequest = 400
  let unauthorized = 401
  let forbidden = 403
  let notFound = 404
  let conflict = 409
  let internalError = 500
  let notImplemented = 501
  let serviceUnavailable = 503

type 'a Result = {
  status: int
  message: string
  body: 'a option
}

module Successful =
  let ok body =
    { status = Codes.ok
      message = ""
      body = Some body
    }

  let created body =
    { status = Codes.created
      message = ""
      body = Some body
    }

module RequestErrors =
  let notFound msg: 'a Result =
    { status = Codes.notFound
      message = msg
      body = None
    }

module ServerErrors =
  let internalError msg: 'a Result =
    { status = Codes.internalError
      message = msg
      body = None
    }