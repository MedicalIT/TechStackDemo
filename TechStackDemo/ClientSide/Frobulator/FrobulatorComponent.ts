namespace Components {
    export namespace Frobulator {

        export const componentName = 'frobulator';
        
        interface Parameters {
            counters?: string[];
        }

        class ViewModel {
            constructor(private initialCounters: string[]) {

            }

            public counterIds = ko.observableArray(this.initialCounters);

            public counterValues = ko.pureComputed(() => this.counterIds().map(counterId=> Data.counterRepository.getCounter(counterId)));

            public increment = (id: string) => {
                Data.counterRepository.increment(id);
            }
        }


        (function () {
            ko.components.register(componentName, {
                synchronous: true,
                template: {
                    require: 'text!/ClientSide/Frobulator/FrobulatorComponent.html'
                },
                viewModel: {
                    createViewModel: (params: Parameters) => new ViewModel(
                        (params.counters || []).slice()
                    )
                }
            });

        })();

    }
}