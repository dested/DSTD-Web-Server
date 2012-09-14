
    var Timeouts = new Dictionary;
    var CurrentPage;

    function AddTimeout(panel, timeout) {
        if (Timeouts.exists(panel)) {
            var a = Timeouts.getVal(panel);
            a.push(timeout);
            Timeouts.setVal(panel, a);
        } else {
            var ar = new Array;
            ar.push(timeout);
            Timeouts.add(panel, ar);
        }
    }


    function clearTimeouts(panel) {
        if (Timeouts.exists(panel)) {
            for (var a = 0; a < Timeouts.getVal(panel).length; a++) {
                clearTimeout(Timeouts.getVal(panel)[a]);
            }
            Timeouts.remove(panel);
        }
    }


    function PostPanel(that, where, async) {
//        if(async)
//            alert(async);
        
        var parent;
        parent = document.getElementById(where.split("|")[1]);
        if (parent == null) {
            return;
        }
        clearTimeouts(parent.id);
        var xhr;
        try {
            xhr = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                xhr = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e2) {
                try {
                    xhr = new XMLHttpRequest;
                } catch (e3) {
                    xhr = false;
                }
            }
        }
        xhr.onreadystatechange = function () {if (xhr.readyState == 4) {if (xhr.status == 200) {var xmlDoc = xhr.responseXML;var ss = "";if (xmlDoc.getElementsByTagName("error").length > 0) {parent.innerHTML = xmlDoc.getElementsByTagName("error")[0].childNodes[0].nodeValue;return;}if (xmlDoc.getElementsByTagName("same").length > 0) {if (xmlDoc.getElementsByTagName("javascript")[0].childNodes[0] != null) {eval(xmlDoc.getElementsByTagName("javascript")[0].childNodes[0].nodeValue);}return;}for (var b = 0; b < xmlDoc.getElementsByTagName("html")[0].childNodes.length; b++) {ss += xmlDoc.getElementsByTagName("html")[0].childNodes[b].nodeValue;}if (xmlDoc.getElementsByTagName("where").length > 0) {where = "|" + xmlDoc.getElementsByTagName("where")[0].childNodes[0].nodeValue;}if (where.split("|")[1] == "theBody") {for (var b = 0; b < Timeouts.items().length; b++) {for (var a = 0; a < Timeouts.items()[b].length; a++) {clearTimeout(Timeouts.items()[b][a]);}}document.body.innerHTML = ss;} else {replaceSelf(parent, ss);}if (xmlDoc.getElementsByTagName("javascript")[0].childNodes[0] != null) {eval(xmlDoc.getElementsByTagName("javascript")[0].childNodes[0].nodeValue);}} else {parent.innerHTML = xhr.status;}}};
        
        xhr.open("POST", "/" + CurrentPage + where, (async ? false : true));
        var xml = async ? async : GrabFieldElements(that, parent, 0);
        xml = "<?xml version='1.0' encoding='UTF-8'?><top>" + xml + "</top>";
        xhr.setRequestHeader("Man", "POST " + "/" + CurrentPage + where + " HTTP/1.1");
        xhr.setRequestHeader("Content-Type", "text/xml");
        xhr.setRequestHeader("Content-Length", xml.length);
        xhr.send(xml);
    }

    function getPanel(item) {
        if (item.getAttribute("name") == "Panel") {
            return item;
        }
        if (item.parentNode != null) {
            return getPanel(item.parentNode);
        } else {
            return null;
        }
    }


    function replaceSelf(parent, new_) {
        $(parent).replaceWith(new_);
    }

    var FieldElements;

    function GrabFieldElements(that, start, count) {
        if (count == 0) {
            FieldElements = new Array();
        }
        var nodes = start.childNodes;
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].id) {
                if (nodes[i].type == "checkbox") {
                    if (nodes[i].checked == true) {
                        var b = {id: nodes[i].id, value: escape(nodes[i].value)};
                        FieldElements.push(b);
                    }
                } else {
                    var b = {id: nodes[i].id, value: escape(nodes[i].value)};
                    FieldElements.push(b);
                    if (nodes[i].disabled) {
                        b = {id: nodes[i].id + "|disabled", value: "true"};
                        FieldElements.push(b);
                    }
                }
            }
            if (nodes[i].childNodes.length > 0) {
                GrabFieldElements(that, nodes[i], count + 1);
            }
        }
        if (count == 0) {
            var xml = buildXML(document.getElementById("CurGUID").value, that);
        }
        return xml;
    }


    function buildXML(guid, that) {
        if (!that) 
            FieldElements = new Array();
        
        FieldElements.push({id: "CurrentGUID", value: guid});

        var xml = "<Information>"; 
        if (that) {
            xml += "<Sender Name=\"" + that.id + "\" />";
        }
        for (var a = 0; a < FieldElements.length; a++) {
            xml += "<Property Name=\"" + FieldElements[a].id + "\" Value=\"" + FieldElements[a].value + "\" />";
        }
        xml += "</Information>";
        return xml;
    }

