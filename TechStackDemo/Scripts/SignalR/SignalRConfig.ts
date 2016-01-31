//$.connection.hub.qs = { 'queryStringParam': 'dummy' };

$.connection.hub.logging = true;
$.connection.hub.received(data=> console.log("SignalR Receive:", JSON.stringify(data)));


$(() => {
    
    if (!$.connection.counterHub.client.newCounterValue) {
        debugger;
    }

    //start the SignalR hub connection
    Data.hubStart();
});