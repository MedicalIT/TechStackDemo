namespace Components {
    export namespace Winner {

        export const componentName = 'winner';

        interface Parameters {
            
        }

        class ViewModel {
            constructor() {

            }

            public total = ko.pureComputed(() => {
                var counterIds = Data.counterRepository.getCounterIds();

                var result = _.chain(counterIds)
                    .map(counterId=> Data.counterRepository.getCounter(counterId))
                    .filter(counter=> !!counter() && !!counter().Value)
                    .reduce((agg, counter) => agg + counter().Value, 0)
                    .value();

                return result;
            });

            private resetting = ko.observable(false);
            public reset = () => {
                if (!this.canReset() || this.resetting()) {
                    return;
                }

                this.resetting(true);
                Data.counterRepository.reset().always(()=>this.resetting(false));
            }

            public canReset = ko.pureComputed(() => !this.resetting());
            
        }


        (function () {
            ko.components.register(componentName, {
                synchronous: true,
                template: {
                    require: 'text!/ClientSide/Winner/WinnerComponent.html'
                },
                viewModel: {
                    createViewModel: (params: Parameters) => new ViewModel()
                }
            });

        })();

    }
}