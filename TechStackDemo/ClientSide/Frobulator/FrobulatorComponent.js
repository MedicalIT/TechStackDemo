var Components;
(function (Components) {
    var Frobulator;
    (function (Frobulator) {
        Frobulator.componentName = 'frobulator';
        var ViewModel = (function () {
            function ViewModel(initialCounters) {
                var _this = this;
                this.initialCounters = initialCounters;
                this.counterIds = ko.observableArray(this.initialCounters);
                this.counterValues = ko.pureComputed(function () { return _this.counterIds().map(function (counterId) { return Data.counterRepository.getCounter(counterId); }); });
                this.increment = function (id) {
                    Data.counterRepository.increment(id);
                };
            }
            return ViewModel;
        })();
        (function () {
            ko.components.register(Frobulator.componentName, {
                synchronous: true,
                template: {
                    require: 'text!/ClientSide/Frobulator/FrobulatorComponent.html'
                },
                viewModel: {
                    createViewModel: function (params) { return new ViewModel((params.counters || []).slice()); }
                }
            });
        })();
    })(Frobulator = Components.Frobulator || (Components.Frobulator = {}));
})(Components || (Components = {}));
