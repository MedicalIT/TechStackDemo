//SignalR
interface SignalR {
    counterHub: {
        server: {
            getCounterItem: (id: string) => JQueryPromise<Data.CounterItem>;
            increment: (id: string, qty: number) => JQueryPromise<Data.CounterItem>,
            decrement: (id: string, qty: number) => JQueryPromise<Data.CounterItem>
        },
        client: {
            allCountersReset: () => void;
            newCounterValues: (values: Data.CounterItem[]) => void;
        }
    }
}


namespace Data {

    export interface CounterItem {
        Id: string;
        Value: string;
    }
    
    
    class CounterRepository {

        constructor() {

        }

        public hubClient = {

            allCountersReset: () => {
                this.dict.clear();
            },

            newCounterValues: (values: Data.CounterItem[]) => {
                values.forEach(value=> {
                    var ob = this.dict.getOrAddValue(value.Id, () => ko.observable({ Id: value.Id, Value: undefined }));
                    ob(value);
                });

            }
        };
        

        private dict = new collections.ObservableDictionary<string, KnockoutObservable<CounterItem>>(s=> s.toLowerCase());
        private subscribed = new collections.Set<string>(s=> s.toLowerCase());

        public getCounter = (id: string) => {
            var result = this.dict.getOrAddValue(id, () => {
                var newItem: CounterItem = {
                    Id: id,
                    Value: undefined  //undefined whilst not present
                }
                return ko.observable(newItem);
            });

            if (!this.subscribed.contains(id)) {
                Data.hubStart().then(() => $.connection.counterHub.server.getCounterItem(id)).then(
                    ctr=> this.dict.getOrAddValue(id,
                        () => ko.observable(ctr),
                        existingCtrObservable=> existingCtrObservable(ctr)
                    )
                );
            }

            return result;
        }

        public increment = (id: string) => {
            Data.hubStart().then(() => $.connection.counterHub.server.increment(id, 1));  //throw away the result: we'll just wait for notification
        }
        
    }

    export var counterRepository = new CounterRepository();

    //wire up listener for the counter repository
    $.connection.counterHub.client = counterRepository.hubClient;
    


    export function hubStart(onStart?: () => void) {        
        var promise = $.connection.hub.start();
        if (onStart) {
            return promise.then(() => onStart());
        }
        return promise;
    }
}