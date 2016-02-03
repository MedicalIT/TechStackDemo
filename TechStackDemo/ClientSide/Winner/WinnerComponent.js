var Components;
(function (Components) {
    var Winner;
    (function (Winner) {
        Winner.componentName = 'winner';
        var ViewModel = (function () {
            function ViewModel() {
                var _this = this;
                this.total = ko.pureComputed(function () {
                    var counterIds = Data.counterRepository.getCounterIds();
                    var result = _.chain(counterIds)
                        .map(function (counterId) { return Data.counterRepository.getCounter(counterId); })
                        .filter(function (counter) { return !!counter() && !!counter().Value; })
                        .reduce(function (agg, counter) { return agg + counter().Value; }, 0)
                        .value();
                    return result;
                });
                this.resetting = ko.observable(false);
                this.reset = function () {
                    if (!_this.canReset() || _this.resetting()) {
                        return;
                    }
                    _this.resetting(true);
                    Data.counterRepository.reset().always(function () { return _this.resetting(false); });
                };
                this.canReset = ko.pureComputed(function () { return !_this.resetting(); });
            }
            return ViewModel;
        })();
        (function () {
            ko.components.register(Winner.componentName, {
                synchronous: true,
                template: {
                    require: 'text!/ClientSide/Winner/WinnerComponent.html'
                },
                viewModel: {
                    createViewModel: function (params) { return new ViewModel(); }
                }
            });
        })();
    })(Winner = Components.Winner || (Components.Winner = {}));
})(Components || (Components = {}));
//# sourceMappingURL=WinnerComponent.js.map