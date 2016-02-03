//$.connection.hub.qs = { 'queryStringParam': 'dummy' };
$.connection.hub.logging = true;
$.connection.hub.received(function (data) { return console.log("SignalR Receive:", JSON.stringify(data)); });
$(function () {
    if (!$.connection.counterHub.client.newCounterValue) {
        debugger;
    }
    //start the SignalR hub connection
    Data.hubStart();
});
//# sourceMappingURL=SignalRConfig.js.map