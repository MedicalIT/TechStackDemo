var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var collections;
(function (collections) {
    var ObservableSet = (function (_super) {
        __extends(ObservableSet, _super);
        function ObservableSet(toStringFunction) {
            _super.call(this, toStringFunction);
            this.observableItems = ko.observableArray([]);
            this.toStr = toStringFunction || collections.defaultToString;
        }
        ObservableSet.prototype.add = function (element) {
            var result = _super.prototype.add.call(this, element);
            if (result) {
                this.observableItems.push(element);
            }
            return result;
        };
        ObservableSet.prototype.remove = function (element) {
            var _this = this;
            var result = _super.prototype.remove.call(this, element);
            if (result) {
                var asString = this.toStr(element);
                this.observableItems.remove(function (item) { return _this.toStr(item) == asString; });
            }
            return result;
        };
        return ObservableSet;
    })(collections.Set);
    collections.ObservableSet = ObservableSet;
    var ObservableDictionary = (function (_super) {
        __extends(ObservableDictionary, _super);
        function ObservableDictionary(toStringFunction, rateLimit) {
            _super.call(this, toStringFunction);
            this.rateLimit = rateLimit;
            this._observableKeys = ko.observableArray([]);
            this.observableKeysImmediate = this._observableKeys;
            this.observableKeys = isDefined(this.rateLimit) ? this.observableKeysImmediate.rateLimitNotifyWhenStop(this.rateLimit) : this.observableKeysImmediate;
            this.localToStr = toStringFunction || collections.defaultToString;
        }
        ObservableDictionary.prototype.clear = function () {
            _super.prototype.clear.call(this);
            this._observableKeys([]);
        };
        ObservableDictionary.prototype.setValue = function (key, value) {
            var alreadyContained = this.containsKey(key);
            var result = _super.prototype.setValue.call(this, key, value);
            if (!alreadyContained) {
                this._observableKeys.push(key);
            }
            return result;
        };
        ObservableDictionary.prototype.remove = function (key) {
            var _this = this;
            var alreadyContained = this.containsKey(key);
            var result = _super.prototype.remove.call(this, key);
            if (alreadyContained) {
                var keyString = this.localToStr(key);
                this._observableKeys.remove(function (item) { return _this.localToStr(item) == keyString; });
            }
            return result;
        };
        ObservableDictionary.prototype.removeMultiple = function (keys) {
            var _this = this;
            if (!keys) {
                return [];
            }
            if (!_.isArray(keys)) {
                return this.removeMultiple([keys]);
            }
            else {
                var existingKeyStrings = keys.filter(function (k) { return _this.containsKey(k); }).map(function (k) { return _this.localToStr(k); });
                var result = keys.map(function (k) { return _super.prototype.remove.call(_this, k); });
                if (existingKeyStrings.length) {
                    this._observableKeys.remove(function (item) { return existingKeyStrings.indexOf(_this.localToStr(item)) >= 0; });
                }
                return result;
            }
        };
        ObservableDictionary.prototype.setValues = function (keysValues) {
            var _this = this;
            var keysNotAlreadyContained = keysValues.filter(function (kv) { return !_this.containsKey(kv.key); }).map(function (kv) { return kv.key; });
            keysValues.forEach(function (kv) { return _super.prototype.setValue.call(_this, kv.key, kv.value); });
            if (keysNotAlreadyContained.length) {
                var underlyingObservableKeys = this._observableKeys();
                keysNotAlreadyContained.forEach(function (key) { return underlyingObservableKeys.push(key); });
                this._observableKeys.valueHasMutated();
            }
        };
        ObservableDictionary.prototype.setValuesInferKeys = function (values, valueToKeyFunc) {
            var keysValues = values.map(function (v) { return ({ key: valueToKeyFunc(v), value: v }); });
            this.setValues(keysValues);
        };
        ObservableDictionary.prototype.getOrAddValues = function (keyValueFuncs) {
            var _this = this;
            //Find keys for which we don't already have a value
            var keysNotAlreadyContained = keyValueFuncs.filter(function (kv) { return !_this.containsKey(kv.key); });
            if (keysNotAlreadyContained.length) {
                //Evaluate them to get values and do one big push into the array, then fire observable keys change
                var underlyingObservableKeys = this._observableKeys();
                keysNotAlreadyContained.forEach(function (keyValueFunc) {
                    var value = keyValueFunc.valueFunc();
                    _super.prototype.setValue.call(_this, keyValueFunc.key, value);
                });
                this._observableKeys.valueHasMutated();
            }
            //go fetch values from the dictionary
            var result = keyValueFuncs.map(function (kvf) { return { key: kvf.key, value: _this.getValue(kvf.key) }; });
            return result;
        };
        return ObservableDictionary;
    })(collections.Dictionary);
    collections.ObservableDictionary = ObservableDictionary;
})(collections || (collections = {}));
