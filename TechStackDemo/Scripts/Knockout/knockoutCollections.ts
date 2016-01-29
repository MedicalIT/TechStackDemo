module collections {
    export class ObservableSet<T> extends collections.Set<T>{


        
        constructor(toStringFunction?: (item: T) => string) {
            super(toStringFunction);
            this.toStr = toStringFunction || collections.defaultToString;

        }
        private toStr: (key: T) => string;
        public observableItems: KnockoutObservableArray<T> = ko.observableArray([]);

        public add(element: T): boolean {
            var result = super.add(element);
            if (result) {
                this.observableItems.push(element);
            }         

            return result;   
        }

        public remove(element: T): boolean {
            var result = super.remove(element);

            if (result) {
                var asString = this.toStr(element);
                this.observableItems.remove((item:T)=>this.toStr(item) == asString);
            }            

            return result;
        }
    }

    export class ObservableDictionary<K, V> extends collections.Dictionary<K, V> {
        constructor(toStringFunction?: (key: K) => string, public rateLimit?: number) {
            super(toStringFunction);
            this.localToStr = toStringFunction || collections.defaultToString;
        }
        private localToStr: (key: K) => string;

        private _observableKeys = ko.observableArray([]);
        public observableKeysImmediate: KnockoutObservable<K[]> = this._observableKeys;
        public observableKeys: KnockoutObservable<K[]> = isDefined(this.rateLimit) ? this.observableKeysImmediate.rateLimitNotifyWhenStop(this.rateLimit) : this.observableKeysImmediate;
        
        public clear() {            
            super.clear();
            this._observableKeys([]);
        }

        public setValue(key: K, value: V): V {
            var alreadyContained = this.containsKey(key);
            var result = super.setValue(key, value);
            if (!alreadyContained) {
                this._observableKeys.push(key);
            }
            return result;
        }

        public remove(key: K): V {
            var alreadyContained = this.containsKey(key);
            var result = super.remove(key);
            if (alreadyContained) {
                var keyString = this.localToStr(key);
                this._observableKeys.remove((item: K) => this.localToStr(item) == keyString);
            }
            return result;
        }

        public removeMultiple(keys: K[] | K): V[] {
            if (!keys) {
                return [];
            }

            if (!_.isArray(keys)) {
                return this.removeMultiple([keys]);
            } else {
                var existingKeyStrings = keys.filter(k=> this.containsKey(k)).map(k=> this.localToStr(k));
                var result = keys.map(k=> super.remove(k));
                if (existingKeyStrings.length) {
                    this._observableKeys.remove((item: K) => existingKeyStrings.indexOf(this.localToStr(item)) >= 0);
                }
                return result;
            }
        }

        public setValues(keysValues: { key: K, value: V }[]) {
            var keysNotAlreadyContained = keysValues.filter(kv=> !this.containsKey(kv.key)).map(kv=> kv.key);

            keysValues.forEach(kv=> super.setValue(kv.key, kv.value));

            if (keysNotAlreadyContained.length) {
                var underlyingObservableKeys = this._observableKeys();
                keysNotAlreadyContained.forEach(key=> underlyingObservableKeys.push(key));
                this._observableKeys.valueHasMutated();
            }
        }

        public setValuesInferKeys(values: V[], valueToKeyFunc: (value:V)=>K) {
            var keysValues = values.map(v=> ({ key: valueToKeyFunc(v), value: v }));
            this.setValues(keysValues);
        }

        public getOrAddValues(keyValueFuncs: { key: K, valueFunc: () => V }[]): { key: K, value: V }[]{
            //Find keys for which we don't already have a value
            var keysNotAlreadyContained = keyValueFuncs.filter(kv=> !this.containsKey(kv.key));            

            if (keysNotAlreadyContained.length) {
                //Evaluate them to get values and do one big push into the array, then fire observable keys change

                var underlyingObservableKeys = this._observableKeys();
                keysNotAlreadyContained.forEach(keyValueFunc=> {
                    var value = keyValueFunc.valueFunc();
                    super.setValue(keyValueFunc.key, value);
                });
                this._observableKeys.valueHasMutated();
            }
            
            //go fetch values from the dictionary
            var result = keyValueFuncs.map(kvf=> { return { key: kvf.key, value: this.getValue(kvf.key) } });
            return result;
        }

        //public removeValues(keys: K[]) {
        //    var keysContained = keys.filter(k=> this.containsKey(k));

        //    if (keysContained.length) {
        //        var underlyingObservableKeys = this.observableKeys();
        //        _(underlyingObservableKeys).
        //    }

        //}
    }
}