$.connection.hub.qs = { 'queryStringParam': 'dummy' };



$(() => {
    //start the SignalR hub connection
    Data.hubStart();
});