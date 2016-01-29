var Data;
(function (Data) {
    var CounterRepository = (function () {
        function CounterRepository() {
            var _this = this;
            this.hubClient = {
                allCountersReset: function () {
                    _this.dict.clear();
                },
                newCounterValues: function (values) {
                    values.forEach(function (value) {
                        var ob = _this.dict.getOrAddValue(value.Id, function () { return ko.observable({ Id: value.Id, Value: undefined }); });
                        ob(value);
                    });
                }
            };
            this.dict = new collections.ObservableDictionary(function (s) { return s.toLowerCase(); });
            this.subscribed = new collections.Set(function (s) { return s.toLowerCase(); });
            this.getCounter = function (id) {
                var result = _this.dict.getOrAddValue(id, function () {
                    var newItem = {
                        Id: id,
                        Value: undefined //undefined whilst not present
                    };
                    return ko.observable(newItem);
                });
                if (!_this.subscribed.contains(id)) {
                    Data.hubStart().then(function () { return $.connection.counterHub.server.getCounterItem(id); }).then(function (ctr) { return _this.dict.getOrAddValue(id, function () { return ko.observable(ctr); }, function (existingCtrObservable) { return existingCtrObservable(ctr); }); });
                }
                return result;
            };
            this.increment = function (id) {
                Data.hubStart().then(function () { return $.connection.counterHub.server.increment(id, 1); }); //throw away the result: we'll just wait for notification
            };
        }
        return CounterRepository;
    })();
    Data.counterRepository = new CounterRepository();
    //wire up listener for the counter repository
    $.connection.counterHub.client = Data.counterRepository.hubClient;
    function hubStart(onStart) {
        var promise = $.connection.hub.start();
        if (onStart) {
            return promise.then(function () { return onStart(); });
        }
        return promise;
    }
    Data.hubStart = hubStart;
})(Data || (Data = {}));
