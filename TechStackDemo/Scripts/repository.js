var Data;
(function (Data) {
    var CounterRepository = (function () {
        function CounterRepository() {
            var _this = this;
            this.hubClient = {
                allCountersReset: function () {
                    _this.dict.forEach(function (key, ob) { return ob({ Id: key, Value: undefined }); });
                },
                newCounterValues: function (values) {
                    values.forEach(function (value) { return _this.ensureInDict(value.Id)(value); });
                }
            };
            this.dict = new collections.ObservableDictionary(function (s) { return s.toLowerCase(); });
            this.ensureInDict = function (id) { return _this.dict.getOrAddValue(id, function () { return ko.observable({ Id: id, Value: undefined }); }); };
            this.subscribed = new collections.Set(function (s) { return s.toLowerCase(); });
            this.getCounter = function (id) {
                var result = _this.ensureInDict(id);
                if (_this.subscribed.add(id)) {
                    //get initial value...
                    Data.hubStart().then(function () { return $.connection.counterHub.server.getCounterItem(id); }).then(function (ctr) { return _this.dict.getOrAddValue(id, function () { return ko.observable(ctr); }, function (existingCtrObservable) { return existingCtrObservable(ctr); }); });
                }
                return result;
            };
            this.increment = function (id, qty) {
                if (qty === void 0) { qty = 1; }
                Data.hubStart().then(function () { return $.connection.counterHub.server.increment(id, qty); }); //throw away the result: we'll just wait for notification
            };
            this.getCounterIds = ko.pureComputed(function () { return _this.dict.observableKeys(); });
            this.reset = function () {
                return Data.hubStart().then(function () { return $.connection.counterHub.server.reset(); }).then(function () { return _this.hubClient.allCountersReset(); });
            };
        }
        return CounterRepository;
    })();
    Data.counterRepository = new CounterRepository();
    //wire up listener for the counter repository
    //$.connection.counterHub.client = _.extend($.connection.counterHub.client, counterRepository.hubClient);
    $.connection.counterHub.client.allCountersReset = function () { return Data.counterRepository.hubClient.allCountersReset(); };
    $.connection.counterHub.client.newCounterValues = function (values) { return Data.counterRepository.hubClient.newCounterValues(values); };
    $.connection.counterHub.client.newCounterValue = function (value) { return Data.counterRepository.hubClient.newCounterValues([value]); };
    //(<any>$.connection.counterHub).on('newCounterValue', (value) => counterRepository.hubClient.newCounterValues([value]));
    var started;
    function hubStart(onStart) {
        if (!started) {
            started = $.connection.hub.start();
        }
        if (onStart) {
            return started.then(function () { return onStart(); });
        }
        return started;
    }
    Data.hubStart = hubStart;
})(Data || (Data = {}));
//# sourceMappingURL=repository.js.map