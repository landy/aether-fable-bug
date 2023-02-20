module TestAetherFeliz.Client.Pages.Index

open Aether
open Feliz
open Elmish
open TestAetherFeliz.Client.Server
open Feliz.UseElmish

type State = {
    Message : string
}

module private State =
    let message_ = Lens ((fun s -> s.Message), (fun value state -> { state with Message = value }))

type private Msg =
    | AskForMessage of bool
    | MessageReceived of ServerResult<string>

let private init () = { Message = "Click on one of the buttons!" }, Cmd.none

let private update (msg:Msg) (model:State) : State * Cmd<Msg> =
    match msg with
    | AskForMessage success -> model, Cmd.OfAsync.eitherAsResult (fun _ -> service.GetMessage success) MessageReceived
    | MessageReceived (Ok msg) -> { model with Message = $"Got success response: {msg}" }, Cmd.none
    | MessageReceived (Error error) -> { model with Message = $"Got server error: {error}" }, Cmd.none

[<ReactComponent>]
let MessageDisplay (state: State) (optic: Lens<State, string>) =
    let message = Optic.get optic state
    Html.div message

[<ReactComponent>]
let IndexView () =
    let state, dispatch = React.useElmish(init, update, [| |])
    let stateMessage = Optic.get State.message_ state


    React.fragment [
        MessageDisplay state State.message_

    ]

