var $= function (str) {
    $_native_writer_write(String(str));
};
var $load = function (path) {
    path = String(path);
    $_native_loadedJS += '|' + path;
    return new Function($_native_loadJS(path));
};
var $debug = function () {
    $(htmlEncode($_debug_locals));
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
var $_native_loadedJS="JSoonConsole/actions.htm.jsoon.js";
var $cur="JSoonConsole/actions.htm.jsoon.js";
var actions=[{"Name":"Dev/AboutActions","Comment":"\n            【开发时接口】查看头哥对Actions的说明\n            ","Parameters":[{"Type":"System.String","Name":"redirect","DefaultValue":"/Dev/Actions.htm","Comment":"请勿更改此参数，请直接点击Execute."}],"Outputs":[]},{"Name":"Dev/GoHome","Comment":"\n            【开发时接口】返回首页\n            ","Parameters":[{"Type":"System.String","Name":"redirect","DefaultValue":"../","Comment":"请勿更改此参数，请直接点击Execute."}],"Outputs":[]},{"Name":"Dev/TestRobot","Comment":"\n            【开发时接口】和机器人对话\n            ","Parameters":[{"Type":"System.String","Name":"salut","DefaultValue":"","Comment":"你要对机器人说的话."}],"Outputs":[{"Type":"System.String[]&","Name":"replies","DefaultValue":"","Comment":"No document available..."}]},{"Name":"Error/","Comment":"\n            用来处理服务器错误的一个方法，仅仅把错误消息存到Cookie里然后跳转。\n            ","Parameters":[{"Type":"System.String","Name":"message","DefaultValue":"","Comment":"message，出错信息传递过来的一个参数"},{"Type":"System.String","Name":"from","DefaultValue":"","Comment":"from，出错信息传递过来的一个参数"}],"Outputs":[]},{"Name":"Admin/VerifyPayment","Comment":"\n            【未实现】确认一个订单的支付宝已经付款.\n            ","Parameters":[{"Type":"System.Int32","Name":"orderId","DefaultValue":"","Comment":"The order id."}],"Outputs":[]},{"Name":"Admin/AddSender","Comment":"\n            【未实现】增加一个送货员.\n            ","Parameters":[{"Type":"System.String","Name":"weiboName","DefaultValue":"","Comment":"送货员的微博昵称."},{"Type":"System.String","Name":"alias","DefaultValue":"","Comment":"送货员标号(英文)."},{"Type":"System.Int32","Name":"interval","DefaultValue":"","Comment":"送货员可接受两个不同楼宇之间订单的时间间隔（分钟）."},{"Type":"System.Int32","Name":"intervalConnected","DefaultValue":"","Comment":"送货员可接受同一个楼宇之间订单的时间间隔（分钟）."}],"Outputs":[{"Type":"System.Int32&","Name":"senderId","DefaultValue":"","Comment":"送货员的ID."}]},{"Name":"Admin/AddBarcode","Comment":"\n            【暂不使用】【未实现】增加一种商品.\n            ","Parameters":[{"Type":"System.String","Name":"barcode","DefaultValue":"","Comment":"商品的包装条形码."},{"Type":"System.String","Name":"title","DefaultValue":"","Comment":"商品名称."},{"Type":"System.Int32","Name":"picNum","DefaultValue":"","Comment":"商品图片的数目."},{"Type":"System.Int32","Name":"productId","DefaultValue":"","Comment":"商品种类ID."}],"Outputs":[]},{"Name":"Admin/Sync","Comment":"\n            【仅服务器端调用】同步指定产品.\n            ","Parameters":[{"Type":"System.Boolean","Name":"enable","DefaultValue":"","Comment":"if set to true 则为增加或修改，否则为删除."},{"Type":"System.String","Name":"url","DefaultValue":"","Comment":"产品展示地址"},{"Type":"System.String","Name":"imgUrl","DefaultValue":"","Comment":"产品缩略图URL(删除时可以不用指定)"},{"Type":"System.String","Name":"title","DefaultValue":"","Comment":"产品标题(删除时可以不用指定)"}],"Outputs":[]},{"Name":"Admin/AddStock","Comment":"\n            在当前区域将一种商品上架.\n            ","Parameters":[{"Type":"System.String","Name":"barcode","DefaultValue":"","Comment":"商品条形码."},{"Type":"System.String","Name":"url","DefaultValue":"","Comment":"The URL."},{"Type":"System.Int32","Name":"price","DefaultValue":"","Comment":"商品价格（角）."}],"Outputs":[]},{"Name":"Admin/DelStock","Comment":"\n            在当前区域将一种商品下架.\n            ","Parameters":[{"Type":"System.String","Name":"url","DefaultValue":"","Comment":"The URL."},{"Type":"System.String","Name":"barcode","DefaultValue":"","Comment":"商品条形码."}],"Outputs":[]},{"Name":"Admin/AddAddress","Comment":"\n            【未实现】在当前区域增加一个楼宇.\n            ","Parameters":[{"Type":"System.String","Name":"address","DefaultValue":"","Comment":"楼宇名称."}],"Outputs":[]},{"Name":"Admin/DelAddress","Comment":"\n            【未实现】在当前区域删除一个楼宇.\n            ","Parameters":[{"Type":"System.String","Name":"address","DefaultValue":"","Comment":"楼宇名称."}],"Outputs":[]},{"Name":"Sender/Activate","Comment":"\n            快递员上班.\n            ","Parameters":[{"Type":"System.String","Name":"stop","DefaultValue":"","Comment":"计划下班的时间."}],"Outputs":[]},{"Name":"Sender/Deactivate","Comment":"\n            快递员提前下班.\n            ","Parameters":[{"Type":"System.String","Name":"stop","DefaultValue":"","Comment":"No document available..."}],"Outputs":[]},{"Name":"Sender/Feedback","Comment":"\n            【未实现】对指定的订单进行反馈.\n            ","Parameters":[{"Type":"System.Int32","Name":"orderId","DefaultValue":"","Comment":"订单ID."}],"Outputs":[]},{"Name":"Auth/OAuthValidate","Comment":"\n            供OAuth2.0协议使用的接口；成功后自动跳转到/OAuthFinished.htm\n            ","Parameters":[{"Type":"System.String","Name":"code","DefaultValue":"","Comment":"The code."},{"Type":"System.String","Name":"provider","DefaultValue":"","Comment":"The provider."},{"Type":"System.Boolean","Name":"remember","DefaultValue":"","Comment":"if set to true [记住密码]."}],"Outputs":[]},{"Name":"Auth/Logout","Comment":"\n            退出当前用户的所有会话。\n            ","Parameters":[],"Outputs":[]},{"Name":"Auth/GetLoginUrl","Comment":"\n            获取登陆窗体URL。\n            ","Parameters":[{"Type":"System.Boolean","Name":"remember","DefaultValue":"","Comment":"if set to true [记住密码]."},{"Type":"System.String","Name":"provider","DefaultValue":"","Comment":"The provider."}],"Outputs":[{"Type":"System.String&","Name":"url","DefaultValue":"","Comment":"The URL."}]},{"Name":"Auth/GetStatus","Comment":"\n            获取当前的用户信息。\n            ","Parameters":[],"Outputs":[{"Type":"user&","Name":"me","DefaultValue":"","Comment":"当前用户的信息，如果未登录则为null"},{"Type":"System.String&","Name":"message","DefaultValue":"","Comment":"The message."},{"Type":"sender&","Name":"senderInfo","DefaultValue":"","Comment":"No document available..."},{"Type":"admin&","Name":"adminInfo","DefaultValue":"","Comment":"No document available..."}]},{"Name":"Auth/OAuthNotify","Comment":"\n            供OAuth2.0协议使用的接口；用于接收信息推送。\n            ","Parameters":[{"Type":"System.String","Name":"provider","DefaultValue":"","Comment":"The provider."}],"Outputs":[]},{"Name":"Auth/GetProvidersList","Comment":"\n            获取可用provider列表\n            ","Parameters":[],"Outputs":[{"Type":"System.String[]&","Name":"providers","DefaultValue":"","Comment":"The providers."}]},{"Name":"Public/GetAreas","Comment":"\n            获取所有可用的区域列表.\n            ","Parameters":[],"Outputs":[{"Type":"System.String[]&","Name":"areas","DefaultValue":"","Comment":"区域列表."}]},{"Name":"Public/GetAddresses","Comment":"\n            获取一个区域可用的楼宇列表.\n            ","Parameters":[{"Type":"System.String","Name":"area","DefaultValue":"","Comment":"当前区域名."}],"Outputs":[{"Type":"System.String[]&","Name":"addresses","DefaultValue":"","Comment":"楼宇列表."}]},{"Name":"Public/GetPrices","Comment":"No document available...","Parameters":[{"Type":"System.String","Name":"url","DefaultValue":"","Comment":"No document available..."}],"Outputs":[{"Type":"System.Collections.Generic.Dictionary`2[System.String,GebimaiService.Public+StockInfo]&","Name":"prices","DefaultValue":"","Comment":"No document available..."}]},{"Name":"User/SetProfile","Comment":"\n            【未实现】设置当前用户的收获地址和支付方式\n            ","Parameters":[{"Type":"System.String","Name":"area","DefaultValue":"","Comment":"用户所在区域"},{"Type":"System.String","Name":"address","DefaultValue":"","Comment":"用户所在楼宇"},{"Type":"System.String","Name":"address2","DefaultValue":"","Comment":"用户所在门牌号"},{"Type":"System.String","Name":"alipay","DefaultValue":"","Comment":"用于付款的支付宝账号，可以留空，留空的话则为货到付款"}],"Outputs":[]}];
var $_debug_locals="\r\n{\r\n\"$_native_web_Root\":\"C:\\\\Users\\\\LayersSss\\\\Documents\\\\Visual Studio 2010\\\\Projects\\\\PsudoSoho\\\\GebimaiService\\\\\",\r\n\"$_native_loadedJS\":\"JSoonConsole/actions.htm.jsoon.js\",\r\n\"$cur\":\"JSoonConsole/actions.htm.jsoon.js\",\r\n\"actions\":\r\n[\r\n\r\n{\r\n\t\"Name\":\"Dev/AboutActions\",\r\n\t\"Comment\":\"\\n            【开发时接口】查看头哥对Actions的说明\\n            \",\r\n\t\"Parameters\":\r\n\t[\r\n\t\r\n\t{\r\n\t\t\"Type\":\"System.String\",\r\n\t\t\"Name\":\"redirect\",\r\n\t\t\"DefaultValue\":\"/Dev/Actions.htm\",\r\n\t\t\"Comment\":\"请勿更改此参数，请直接点击Execute.\"\r\n\t}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Dev/GoHome\",\r\n\"Comment\":\"\\n            【开发时接口】返回首页\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"redirect\",\r\n\"DefaultValue\":\"../\",\r\n\"Comment\":\"请勿更改此参数，请直接点击Execute.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Dev/TestRobot\",\r\n\"Comment\":\"\\n            【开发时接口】和机器人对话\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"salut\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"你要对机器人说的话.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String[]&\",\r\n\"Name\":\"replies\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"No document available...\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Error/\",\r\n\"Comment\":\"\\n            用来处理服务器错误的一个方法，仅仅把错误消息存到Cookie里然后跳转。\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"message\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"message，出错信息传递过来的一个参数\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"from\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"from，出错信息传递过来的一个参数\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/VerifyPayment\",\r\n\"Comment\":\"\\n            【未实现】确认一个订单的支付宝已经付款.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.Int32\",\r\n\"Name\":\"orderId\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The order id.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/AddSender\",\r\n\"Comment\":\"\\n            【未实现】增加一个送货员.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"weiboName\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"送货员的微博昵称.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"alias\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"送货员标号(英文).\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.Int32\",\r\n\"Name\":\"interval\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"送货员可接受两个不同楼宇之间订单的时间间隔（分钟）.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.Int32\",\r\n\"Name\":\"intervalConnected\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"送货员可接受同一个楼宇之间订单的时间间隔（分钟）.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.Int32&\",\r\n\"Name\":\"senderId\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"送货员的ID.\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/AddBarcode\",\r\n\"Comment\":\"\\n            【暂不使用】【未实现】增加一种商品.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"barcode\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"商品的包装条形码.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"title\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"商品名称.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.Int32\",\r\n\"Name\":\"picNum\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"商品图片的数目.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.Int32\",\r\n\"Name\":\"productId\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"商品种类ID.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/Sync\",\r\n\"Comment\":\"\\n            【仅服务器端调用】同步指定产品.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.Boolean\",\r\n\"Name\":\"enable\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"if set to true 则为增加或修改，否则为删除.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"url\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"产品展示地址\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"imgUrl\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"产品缩略图URL(删除时可以不用指定)\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"title\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"产品标题(删除时可以不用指定)\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/AddStock\",\r\n\"Comment\":\"\\n            在当前区域将一种商品上架.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"barcode\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"商品条形码.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"url\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The URL.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.Int32\",\r\n\"Name\":\"price\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"商品价格（角）.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/DelStock\",\r\n\"Comment\":\"\\n            在当前区域将一种商品下架.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"url\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The URL.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"barcode\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"商品条形码.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/AddAddress\",\r\n\"Comment\":\"\\n            【未实现】在当前区域增加一个楼宇.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"address\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"楼宇名称.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Admin/DelAddress\",\r\n\"Comment\":\"\\n            【未实现】在当前区域删除一个楼宇.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"address\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"楼宇名称.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Sender/Activate\",\r\n\"Comment\":\"\\n            快递员上班.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"stop\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"计划下班的时间.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Sender/Deactivate\",\r\n\"Comment\":\"\\n            快递员提前下班.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"stop\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"No document available...\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Sender/Feedback\",\r\n\"Comment\":\"\\n            【未实现】对指定的订单进行反馈.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.Int32\",\r\n\"Name\":\"orderId\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"订单ID.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Auth/OAuthValidate\",\r\n\"Comment\":\"\\n            供OAuth2.0协议使用的接口；成功后自动跳转到/OAuthFinished.htm\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"code\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The code.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"provider\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The provider.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.Boolean\",\r\n\"Name\":\"remember\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"if set to true [记住密码].\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Auth/Logout\",\r\n\"Comment\":\"\\n            退出当前用户的所有会话。\\n            \",\r\n\"Parameters\":\r\n[\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Auth/GetLoginUrl\",\r\n\"Comment\":\"\\n            获取登陆窗体URL。\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.Boolean\",\r\n\"Name\":\"remember\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"if set to true [记住密码].\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"provider\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The provider.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String&\",\r\n\"Name\":\"url\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The URL.\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Auth/GetStatus\",\r\n\"Comment\":\"\\n            获取当前的用户信息。\\n            \",\r\n\"Parameters\":\r\n[\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"user&\",\r\n\"Name\":\"me\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"当前用户的信息，如果未登录则为null\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String&\",\r\n\"Name\":\"message\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The message.\"\r\n}\r\n,\r\n{\r\n\"Type\":\"sender&\",\r\n\"Name\":\"senderInfo\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"No document available...\"\r\n}\r\n,\r\n{\r\n\"Type\":\"admin&\",\r\n\"Name\":\"adminInfo\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"No document available...\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Auth/OAuthNotify\",\r\n\"Comment\":\"\\n            供OAuth2.0协议使用的接口；用于接收信息推送。\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"provider\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The provider.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Auth/GetProvidersList\",\r\n\"Comment\":\"\\n            获取可用provider列表\\n            \",\r\n\"Parameters\":\r\n[\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String[]&\",\r\n\"Name\":\"providers\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"The providers.\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Public/GetAreas\",\r\n\"Comment\":\"\\n            获取所有可用的区域列表.\\n            \",\r\n\"Parameters\":\r\n[\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String[]&\",\r\n\"Name\":\"areas\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"区域列表.\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Public/GetAddresses\",\r\n\"Comment\":\"\\n            获取一个区域可用的楼宇列表.\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"area\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"当前区域名.\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String[]&\",\r\n\"Name\":\"addresses\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"楼宇列表.\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"Public/GetPrices\",\r\n\"Comment\":\"No document available...\",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"url\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"No document available...\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.Collections.Generic.Dictionary`2[System.String,GebimaiService.Public+StockInfo]&\",\r\n\"Name\":\"prices\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"No document available...\"\r\n}\r\n]\r\n}\r\n,\r\n{\r\n\"Name\":\"User/SetProfile\",\r\n\"Comment\":\"\\n            【未实现】设置当前用户的收获地址和支付方式\\n            \",\r\n\"Parameters\":\r\n[\r\n\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"area\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"用户所在区域\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"address\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"用户所在楼宇\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"address2\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"用户所在门牌号\"\r\n}\r\n,\r\n{\r\n\"Type\":\"System.String\",\r\n\"Name\":\"alipay\",\r\n\"DefaultValue\":\"\",\r\n\"Comment\":\"用于付款的支付宝账号，可以留空，留空的话则为货到付款\"\r\n}\r\n],\r\n\"Outputs\":\r\n[\r\n]\r\n}\r\n]\r\n}";