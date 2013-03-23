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
var $_native_loadedJS="JSoonConsole/debuggers.htm.jsoon.js";
var $cur="JSoonConsole/debuggers.htm.jsoon.js";
var debuggers={"admin.jsoon.js":{"Locals":[],"SystemLocals":[],"BreakPoints":[{"Condition":null,"Position":66},{"Condition":null,"Position":68},{"Condition":null,"Position":102},{"Condition":null,"Position":127},{"Condition":null,"Position":146},{"Condition":null,"Position":175},{"Condition":null,"Position":177},{"Condition":null,"Position":184}],"CurrentPosition":-1,"Source":"/// <reference path=\"/JSoonReferences/admin.jsoon.js\" />\r\n/*<!--*/\r\n$load('Default.htm.master.js')({\r\n    body: function () {\r\n        $debug();\r\n        /*-->NewLiteral<!--*/\r\n    }\r\n});  //-->"},"JSoonConsole/Default.htm.jsoon.js":{"Locals":[],"SystemLocals":[],"BreakPoints":[{"Condition":null,"Position":83},{"Condition":null,"Position":85},{"Condition":null,"Position":87},{"Condition":null,"Position":128},{"Condition":null,"Position":151},{"Condition":null,"Position":176},{"Condition":null,"Position":182},{"Condition":null,"Position":190},{"Condition":null,"Position":215},{"Condition":null,"Position":217},{"Condition":null,"Position":274},{"Condition":null,"Position":276},{"Condition":null,"Position":364},{"Condition":null,"Position":407},{"Condition":null,"Position":454},{"Condition":null,"Position":456},{"Condition":null,"Position":543},{"Condition":null,"Position":586},{"Condition":null,"Position":588},{"Condition":null,"Position":590},{"Condition":null,"Position":653},{"Condition":null,"Position":655},{"Condition":null,"Position":662},{"Condition":null,"Position":667}],"CurrentPosition":-1,"Source":"/// <reference path=\"/JSoonReferences/JSoonConsole/Default.htm.jsoon.js\">\r\n/*<!--*/\r\n\r\n$load('JSoonConsole/Frame.master.js')({\r\n    title: 'Summary',\r\n    head: function () {\r\n    \r\n    },\r\n    body: function () {\r\n\r\n        /*-->\r\n        <h3>SubPages:</h3>\r\n        <!--*/\r\n        for (var i = 0; i < subPages.length; i++) { $(subPages[i].Folder); /*-->*.<!--*/$(subPages[i].Extension); /*--><br /><!--*/ } /*-->\r\n\r\n<hr />\r\n<h3>Renderers:</h3>\r\n<!--*/\r\n        for (var i = 0; i < renderers.length; i++) { $(renderers[i]); /*--><br /><!--*/ } /*-->\r\n<hr />\r\n<h3>Actions:</h3>\r\n<!--*/\r\n\r\n        $(actions.length);  /*--> registered.\r\n        \r\n<!--*/\r\n    }\r\n});\r\n//-->"}};
var $_debug_locals="\r\n{\r\n\"$_native_web_Root\":\"C:\\\\Users\\\\LayersSss\\\\Documents\\\\Visual Studio 2010\\\\Projects\\\\PsudoSoho\\\\GebimaiService\\\\\",\r\n\"$_native_loadedJS\":\"JSoonConsole/debuggers.htm.jsoon.js\",\r\n\"$cur\":\"JSoonConsole/debuggers.htm.jsoon.js\",\r\n\"debuggers\":\r\n{\r\n\"admin.jsoon.js\":\r\n{\r\n\t\"Locals\":\r\n\t[\r\n\t],\r\n\t\"SystemLocals\":\r\n\t[\r\n\t],\r\n\t\"BreakPoints\":\r\n\t[\r\n\t\r\n\t{\r\n\t\t\"Condition\":null,\r\n\t\t\"Position\":66\r\n\t}\r\n,\r\n{\r\n\t\"Condition\":null,\r\n\t\"Position\":68\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":102\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":127\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":146\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":175\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":177\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":184\r\n}\r\n],\r\n\"CurrentPosition\":-1,\r\n\"Source\":\"/// <reference path=\\\"/JSoonReferences/admin.jsoon.js\\\" />\\r\\n/*<!--*/\\r\\n$load('Default.htm.master.js')({\\r\\n    body: function () {\\r\\n        $debug();\\r\\n        /*-->NewLiteral<!--*/\\r\\n    }\\r\\n});  //-->\"\r\n},\r\n\"JSoonConsole/Default.htm.jsoon.js\":\r\n{\r\n\"Locals\":\r\n[\r\n],\r\n\"SystemLocals\":\r\n[\r\n],\r\n\"BreakPoints\":\r\n[\r\n\r\n{\r\n\"Condition\":null,\r\n\"Position\":83\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":85\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":87\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":128\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":151\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":176\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":182\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":190\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":215\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":217\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":274\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":276\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":364\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":407\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":454\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":456\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":543\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":586\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":588\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":590\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":653\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":655\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":662\r\n}\r\n,\r\n{\r\n\"Condition\":null,\r\n\"Position\":667\r\n}\r\n],\r\n\"CurrentPosition\":-1,\r\n\"Source\":\"/// <reference path=\\\"/JSoonReferences/JSoonConsole/Default.htm.jsoon.js\\\">\\r\\n/*<!--*/\\r\\n\\r\\n$load('JSoonConsole/Frame.master.js')({\\r\\n    title: 'Summary',\\r\\n    head: function () {\\r\\n    \\r\\n    },\\r\\n    body: function () {\\r\\n\\r\\n        /*-->\\r\\n        <h3>SubPages:</h3>\\r\\n        <!--*/\\r\\n        for (var i = 0; i < subPages.length; i++) { $(subPages[i].Folder); /*-->*.<!--*/$(subPages[i].Extension); /*--><br /><!--*/ } /*-->\\r\\n\\r\\n<hr />\\r\\n<h3>Renderers:</h3>\\r\\n<!--*/\\r\\n        for (var i = 0; i < renderers.length; i++) { $(renderers[i]); /*--><br /><!--*/ } /*-->\\r\\n<hr />\\r\\n<h3>Actions:</h3>\\r\\n<!--*/\\r\\n\\r\\n        $(actions.length);  /*--> registered.\\r\\n        \\r\\n<!--*/\\r\\n    }\\r\\n});\\r\\n//-->\"\r\n}\r\n}\r\n}";