var rootNewStrings = '';
var rootNewArrays = '';
for (var p in root) {
    if (typeof (root[p]) == 'string') {
        rootNewStrings += 's_' + p + ',';
    }
    if (typeof (root[p]) == 'number') {
        rootNewStrings += 'n_' + p + ',';
    }
    if (typeof (root[p]) == 'boolean') {
        rootNewStrings += 'b_' + p + ',';
    }
    if (root[p].constructor === Array) {
        rootNewArrays += p + ',';
        if (root[p].length) 
        {
            for (var p2 in root[p][0]) {
                if (typeof (root[p][0][p2]) == 'string') {
                    rootNewArrays += 's_' + p2 + '|';
                }
                if (typeof (root[p][0][p2]) == 'number') {
                    rootNewArrays += 'n_' + p2 + '|';
                }
                if (typeof (root[p][0][p2]) == 'boolean') {
                    rootNewArrays += 'b_' + p2 + '|';
                }
            }
            rootNewArrays += ',';
            rootNewArrays += root[p].length + ',';
        } else {
            rootNewArrays += ',0,';
        }
    }
}

