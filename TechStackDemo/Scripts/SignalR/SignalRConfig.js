$.connection.hub.qs = { 'queryStringParam': 'dummy' };
$(function () {
    //start the SignalR hub connection
    Data.hubStart();
});
