var $= function (str) {
    $_native_writer_write(String(str));
};
var $load = function (path) {
    path = String(path);
    $_native_loadedJS += '|' + path;
    return new Function($_native_loadJS(path));
};

var JSON;
if (!JSON) {
    JSON = {};
}

(function () {
    'use strict';

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function (key) {

            return isFinite(this.valueOf()) ?
                this.getUTCFullYear() + '-' +
                f(this.getUTCMonth() + 1) + '-' +
                f(this.getUTCDate()) + 'T' +
                f(this.getUTCHours()) + ':' +
                f(this.getUTCMinutes()) + ':' +
                f(this.getUTCSeconds()) + 'Z' : null;
        };

        String.prototype.toJSON =
            Number.prototype.toJSON =
            Boolean.prototype.toJSON = function (key) {
                return this.valueOf();
            };
    }
    
    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = { // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"': '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

        // If the string contains no control characters, no quote characters, and no
        // backslash characters, then we can safely slap some quotes around it.
        // Otherwise we must also replace the offending characters with safe escape
        // sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string' ? c :
                '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }


    function str(key, holder) {

        // Produce a string from holder[key].

        var i, // The loop counter.
            k, // The member key.
            v, // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

        // If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

        // If we were called with a replacer function, then call the replacer to
        // obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

        // What happens next depends on the value's type.

        switch (typeof value) {
            case 'string':
                return quote(value);

            case 'number':

                // JSON numbers must be finite. Encode non-finite numbers as null.

                return isFinite(value) ? String(value) : 'null';

            case 'boolean':
            case 'null':

                // If the value is a boolean or null, convert it to a string. Note:
                // typeof null does not produce 'null'. The case is included here in
                // the remote chance that this gets fixed someday.

                return String(value);

                // If the type is 'object', we might be dealing with an object or an array or
                // null.

            case 'object':

                // Due to a specification blunder in ECMAScript, typeof null is 'object',
                // so watch out for that case.

                if (!value) {
                    return 'null';
                }

                // Make an array to hold the partial results of stringifying this object value.

                gap += indent;
                partial = [];

                // Is the value an array?

                if (Object.prototype.toString.apply(value) === '[object Array]') {

                    // The value is an array. Stringify every element. Use null as a placeholder
                    // for non-JSON values.

                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || 'null';
                    }

                    // Join all of the elements together, separated with commas, and wrap them in
                    // brackets.

                    v = partial.length === 0 ? '[]' : gap ?
                    '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' :
                    '[' + partial.join(',') + ']';
                    gap = mind;
                    return v;
                }

                // If the replacer is an array, use it to select the members to be stringified.

                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        k = rep[i];
                        if (typeof k === 'string') {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                } else {

                    // Otherwise, iterate through all of the keys in the object.

                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                }

                // Join all of the member texts together, separated with commas,
                // and wrap them in braces.

                v = partial.length === 0 ? '{}' : gap ?
                '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' :
                '{' + partial.join(',') + '}';
                gap = mind;
                return v;
        }
    }

    // If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

            // The stringify method takes a value and an optional replacer, and an optional
            // space parameter, and returns a JSON text. The replacer can be a function
            // that can replace values, or an array of strings that will select the keys.
            // A default replacer method can be provided. Use of the space parameter can
            // produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

            // If the space parameter is a number, make an indent string containing that
            // many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

                // If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

            // If there is a replacer, it must be a function or an array.
            // Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

            // Make a fake root object containing our value under the key of ''.
            // Return the result of stringifying the value.

            return str('', { '': value });
        };
    }


    // If the JSON object does not yet have a parse method, give it one.

    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

            // The parse method takes a text and an optional reviver function, and returns
            // a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

                // The walk method is used to recursively walk the resulting structure so
                // that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


            // Parsing happens in four stages. In the first stage, we replace certain
            // Unicode characters with escape sequences. JavaScript handles many characters
            // incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

            // In the second stage, we run the text against regular expressions that look
            // for non-JSON patterns. We are especially concerned with '()' and 'new'
            // because they can cause invocation, and '=' because it can cause mutation.
            // But just to be safe, we want to reject all unexpected forms.

            // We split the second stage into 4 regexp operations in order to work around
            // crippling inefficiencies in IE's and Safari's regexp engines. First we
            // replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
            // replace all simple value tokens with ']' characters. Third, we delete all
            // open brackets that follow a colon or comma or that begin the text. Finally,
            // we look to see that the remaining characters are only whitespace or ']' or
            // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                    .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                // In the third stage we use the eval function to compile the text into a
                // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                // in JavaScript: it can begin a block or an object literal. We wrap the text
                // in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

                // In the optional fourth stage, we recursively walk the new structure, passing
                // each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function' ?
                    walk({ '': j }, '') : j;
            }

            // If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }

    // Augment the basic prototypes if they have not already been augmented.
    // These forms are obsolete. It is recommended that JSON.stringify and
    // JSON.parse be used instead.

})();

var $_native_web_Root="C:\\Users\\LayersSss\\Documents\\Visual Studio 2010\\Projects\\PsudoSoho\\GebimaiService\\";
var $_native_loadedJS="admin.jsoon.js";
var $cur="admin.jsoon.js";
var $subPage="touge";
var area="暨南大学珠海校区";
var stocks=[{"area":"暨南大学珠海校区","barcodebc":"1000000000051","enabled":true,"id":40,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000051","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/奥利奥原味家庭装390g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"奥利奥原味家庭装390g","url":"http://gebimai.com/goods/317","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000052","enabled":true,"id":41,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000052","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/统一-阿萨姆奶茶500ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"统一阿萨姆奶茶500ml","url":"http://gebimai.com/goods/320","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000050","enabled":true,"id":42,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000050","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/徐福记草莓酥184g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"徐福记草莓酥184g","url":"http://gebimai.com/goods/308","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000049","enabled":true,"id":43,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000049","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/嘉顿鸡片200g蒜香.jpg","note":null,"num":1,"price":0,"productid":1,"title":"嘉顿鸡片200g蒜香","url":"http://gebimai.com/goods/305","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000048","enabled":true,"id":44,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000048","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/冠生园葱油压缩饼干118g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"冠生园葱油压缩饼干118g","url":"http://gebimai.com/goods/302","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000047","enabled":true,"id":45,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000047","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/日清UFO铁板色拉鱿鱼123g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"日清UFO铁板色拉鱿鱼123g","url":"http://gebimai.com/goods/298","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000044","enabled":true,"id":46,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000044","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/王老吉凉茶310ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"王老吉凉茶310ml","url":"http://gebimai.com/goods/289","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000043","enabled":true,"id":47,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000043","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/冠利苹果醋250ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"冠利苹果醋250ml","url":"http://gebimai.com/goods/286","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000045","enabled":true,"id":48,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000045","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/雀巢丝滑拿铁咖啡饮料268ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"雀巢丝滑拿铁咖啡饮料268ml","url":"http://gebimai.com/goods/292","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000041","enabled":true,"id":49,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000041","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/怡口莲90g咖啡味-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"怡口莲90g咖啡味","url":"http://gebimai.com/goods/278","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000034","enabled":true,"id":50,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000034","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/益达木糖醇口香糖56g薄荷味-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"益达木糖醇口香糖56g薄荷味","url":"http://gebimai.com/goods/225","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000029","enabled":true,"id":51,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000029","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/日清UFO鱼香肉丝炒面124g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"日清UFO鱼香肉丝炒面124g","url":"http://gebimai.com/goods/207","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000026","enabled":true,"id":52,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000026","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/双汇香辣香脆肠35g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"双汇香辣香脆肠35g","url":"http://gebimai.com/goods/190","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000023","enabled":true,"id":53,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000023","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/雀巢咖啡伴侣盒装60g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"雀巢咖啡伴侣盒装60g","url":"http://gebimai.com/goods/169","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000046","enabled":true,"id":54,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000046","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/张君雅和风鸡汁拉面条饼65g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"张君雅和风鸡汁拉面条饼65g","url":"http://gebimai.com/goods/295","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000042","enabled":true,"id":55,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000042","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/荷氏冰球柠薄荷糖15g檬茶口味-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"荷氏冰球柠薄荷糖15g檬茶口味","url":"http://gebimai.com/goods/282","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000031","enabled":true,"id":56,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000031","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/徐福记蛋黄沙琪玛-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"徐福记蛋黄沙琪玛","url":"http://gebimai.com/goods/209","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000032","enabled":true,"id":57,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000032","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/大白兔原味奶糖227g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"大白兔原味奶糖227g","url":"http://gebimai.com/goods/219","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000033","enabled":true,"id":58,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000033","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/曼妥思口香糖葡萄味46g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"曼妥思口香糖葡萄味46g","url":"http://gebimai.com/goods/222","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000027","enabled":true,"id":59,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000027","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/盼盼法式小面包奶香味440g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"盼盼法式小面包奶香味440g","url":"http://gebimai.com/goods/198","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000024","enabled":true,"id":60,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000024","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/出前一丁高品质即食面麻油味100g（香港）-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"出前一丁高品质即食面麻油味100g","url":"http://gebimai.com/goods/173","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000028","enabled":true,"id":61,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000028","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/丘比沙拉酱200g脂肪减半香甜味-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"丘比沙拉酱200g脂肪减半香甜味","url":"http://gebimai.com/goods/204","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000058","enabled":true,"id":62,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000058","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/自然派辣味牛肉条100g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"自然派辣味牛肉条100g","url":"http://gebimai.com/goods/349","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000057","enabled":true,"id":63,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000057","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/自然派蜜汁猪肉条100g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"自然派蜜汁猪肉条100g","url":"http://gebimai.com/goods/346","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000040","enabled":true,"id":64,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000040","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/纤麸高纤消化饼干570g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"纤麸高纤消化饼干570g","url":"http://gebimai.com/goods/275","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000025","enabled":true,"id":65,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000025","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/乐事意大利香浓红烩75g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"乐事意大利香浓红烩75g","url":"http://gebimai.com/goods/165","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000017","enabled":true,"id":66,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000017","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/乐事无限嗞嗞烤肉味110-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"乐事无限嗞嗞烤肉味110g","url":"http://gebimai.com/goods/119","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000008","enabled":true,"id":67,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000008","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/奥利奥缤纷双果树莓蓝莓-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"奥利奥缤纷双果树莓蓝莓","url":"http://gebimai.com/goods/60","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000016","enabled":true,"id":68,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000016","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/乐事无限忠于原味110g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"乐事无限忠于原味110g","url":"http://gebimai.com/goods/115","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000009","enabled":true,"id":69,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000009","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/太平梳打香葱口味-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"太平梳打香葱口味","url":"http://gebimai.com/goods/63","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000037","enabled":true,"id":70,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000037","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/怡口莲臻仁90g袋装-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"怡口莲臻仁90g袋装","url":"http://gebimai.com/goods/236","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000036","enabled":true,"id":71,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000036","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/彩虹果汁糖120g大瓶装-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"彩虹果汁糖120g大瓶装","url":"http://gebimai.com/goods/233","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000035","enabled":true,"id":72,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000035","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/阿尔卑斯牛奶硬糖150g意式咖啡味-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"阿尔卑斯牛奶硬糖150g意式咖啡味","url":"http://gebimai.com/goods/228","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000064","enabled":true,"id":73,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000064","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/甘竹豆豉鱼块184g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"甘竹豆豉鱼块184g","url":"http://gebimai.com/goods/370","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000063","enabled":true,"id":74,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000063","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/陶华碧老干妈风味豆豉280g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"陶华碧老干妈风味豆豉280g","url":"http://gebimai.com/goods/366","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000062","enabled":true,"id":75,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000062","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/李锦记草菇老抽500ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"李锦记草菇老抽500ml","url":"http://gebimai.com/goods/362","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000061","enabled":true,"id":76,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000061","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/粤盐加碘精制盐500-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"粤盐加碘精制盐500","url":"http://gebimai.com/goods/359","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000060","enabled":true,"id":77,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000060","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/潮盛橄榄菜450g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"潮盛橄榄菜450g","url":"http://gebimai.com/goods/355","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000056","enabled":true,"id":78,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000056","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/日清炒面大王红油辣肉风味90g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"日清炒面大王红油辣肉风味90g","url":"http://gebimai.com/goods/343","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000055","enabled":true,"id":79,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000055","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/日清炒面大王红烧牛肉风味90g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"日清炒面大王红烧牛肉风味90g","url":"http://gebimai.com/goods/340","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000054","enabled":true,"id":80,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000054","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/日清炒面大王香辣肉酱风味90g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"日清炒面大王香辣肉酱风味90g","url":"http://gebimai.com/goods/337","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000053","enabled":true,"id":81,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000053","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/日清炒面大王葱油肉丝风味90g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"日清炒面大王葱油肉丝风味90g","url":"http://gebimai.com/goods/334","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000039","enabled":true,"id":82,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000039","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/五谷道场非油炸红烧牛肉面92gx5-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"五谷道场非油炸红烧牛肉面92gx5","url":"http://gebimai.com/goods/243","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000038","enabled":true,"id":83,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000038","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/康师傅红烧牛肉面五连包500g-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"康师傅红烧牛肉面五连包500g","url":"http://gebimai.com/goods/240","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000005","enabled":true,"id":84,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000005","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/康师傅红烧牛肉面桶装-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"康师傅红烧牛肉面桶装","url":"http://gebimai.com/goods/43","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000007","enabled":true,"id":85,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000007","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/康师傅香菇炖鸡面桶装-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"康师傅香菇炖鸡面桶装","url":"http://gebimai.com/goods/56","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000059","enabled":true,"id":86,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000059","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/生力罐装广氏菠萝啤330ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"生力罐装广氏菠萝啤330ml","url":"http://gebimai.com/goods/352","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000014","enabled":true,"id":87,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000014","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/统一绿茶500ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"统一绿茶500ml","url":"http://gebimai.com/goods/109","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000013","enabled":true,"id":88,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000013","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/统一蜜桃多-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"统一蜜桃多450ml","url":"http://gebimai.com/goods/103","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000018","enabled":true,"id":89,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000018","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/可口可乐500ml1-284x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"可口可乐500ml","url":"http://gebimai.com/goods/124","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000015","enabled":true,"id":90,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000015","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/佳得乐冰橘味600ml-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"佳得乐冰橘味600ml","url":"http://gebimai.com/goods/112","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000012","enabled":true,"id":91,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000012","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/果缤纷热带美味-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"果缤纷热带美味450ml","url":"http://gebimai.com/goods/100","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000010","enabled":true,"id":92,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000010","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/统一冰红茶500毫升-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"统一冰红茶500ml","url":"http://gebimai.com/goods/66","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000022","enabled":true,"id":93,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000022","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/qingdao_pijiu-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"青岛啤酒330ml罐装","url":"http://gebimai.com/goods/94","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000020","enabled":true,"id":94,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000020","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/quechao_kafei-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"雀巢咖啡香滑180ml","url":"http://gebimai.com/goods/133","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000021","enabled":true,"id":95,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000021","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/haipai_haitai-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"海牌海苔10包装韩国造","url":"http://gebimai.com/goods/137","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}},{"area":"暨南大学珠海校区","barcodebc":"1000000000019","enabled":true,"id":96,"importprice":0,"price":1,"orders":null,"barcode":{"bc":"1000000000019","imgUrl":"http://gebimai.com/wp-content/uploads/2012/03/huangfeihong_huasheng-300x300.jpg","note":null,"num":1,"price":0,"productid":1,"title":"黄飞红麻辣花生76g","url":"http://gebimai.com/goods/129","stocks":null,"product":{"categoryid":1,"id":1,"isInstante":0,"sort":0,"title":"薯片","barcodes":null,"category":{"id":1,"sort":0,"title":"零食","products":null}}}}];
var senders=[{"addresses":"[\"珠海宿舍1栋\",\"珠海宿舍3栋(男生)\",\"珠海宿舍4栋(男生)\",\"珠海宿舍5栋(男生)\"]","alias":"touge","area":"暨南大学珠海校区","id":6,"interval":40,"intervalConnected":10,"userid":2,"timespans":null,"user":{"address":"珠海宿舍3栋(女生)","address2":"4331","alipay":null,"area":"暨南大学珠海校区","authId":"2051426325","authProvider":"weibo","avatarUrl":"http://tp2.sinaimg.cn/2051426325/50/5607173225/1","gender":"m","id":2,"message":null,"name":"layerssss","username":"2051426325.weibo","admins":null,"dev":null,"senders":null}},{"addresses":"","alias":"biaoge","area":"暨南大学珠海校区","id":8,"interval":10,"intervalConnected":1,"userid":7,"timespans":null,"user":{"address":"珠海宿舍5栋(男生)","address2":"5555","alipay":null,"area":"暨南大学珠海校区","authId":"1618282990","authProvider":"weibo","avatarUrl":"http://tp3.sinaimg.cn/1618282990/50/0/1","gender":"m","id":7,"message":null,"name":"abiaoa","username":"1618282990.weibo","admins":null,"dev":null,"senders":null}}];
var payingOrders=[];
var activeSenders=[];
var addresses=["珠海宿舍1栋","珠海宿舍2栋","珠海宿舍3栋(男生)","珠海宿舍3栋(女生)","珠海宿舍4栋(男生)","珠海宿舍4栋(女生)","珠海宿舍5栋(男生)","珠海宿舍5栋(女生)","珠海宿舍6栋","珠海宿舍7栋","珠海宿舍8栋","珠海宿舍9栋"];