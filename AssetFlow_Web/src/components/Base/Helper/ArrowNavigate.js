
const arrownavigate = (event) => {

    // console.log(event.target);

    switch (event.which) {

        case arrow().up:
            var upInput = getRow(event, "prev");
            if(upInput){
                upInput.focus();
                setTimeout(function(){
                    upInput.select();
                }, 0)
            }
        break;

        case arrow().down:
            var downInput = getRow(event, "next");
            if(downInput){
                downInput.focus();
                setTimeout(function(){
                    downInput.select();
                }, 0)
            }
        break;

        case arrow().left:
            var leftInput = getCell(event, "prev");
            if(leftInput){
                leftInput.focus();
                setTimeout(function(){
                    leftInput.select();
                }, 0)
            }
        break;

        case arrow().right:
            var rightInput = getCell(event, "next");
            if(rightInput){
                rightInput.focus();
                setTimeout(function(){
                    rightInput.select();
                }, 0)
            }
        break;

        // case arrow().backspace:
        //     event.target.value = "";
        // break;
        
        default:
            break;
    }
};

//mengambil element input di bawah atau atas
function getRow(event, type){ 
    var inputElement = null;
    var parent = event.target.parentElement.parentElement;
    var row = getElementSibling(parent, type);

    if(row != null){
        if(parent.tagName.toLowerCase() == 'tr'){
            inputElement = getInput(row.cells[getCellIndex(event)]);
        }
        else{
            var Parent = getParent(parent, type);
            if(Parent.cells){
                inputElement = getInput(Parent.cells[parent.cellIndex]);
            }
        }
        
    }else { return false; }

    return inputElement;
}

//mengambil element input di kanan atau kiri
function getCell(event, type){
    var inputElement = null;
    var parent = event.target.parentElement;
    var cell = getElementSibling(parent, type);

    if(cell != null){
        inputElement = getInput(cell);
    }
    else {
        var Parent = getParent(parent, type);
        inputElement = getInput(Parent);
    }

    return inputElement;
}

//mengambil element td ataupun tr
function getParent(event, type){
    var parent = event.parentElement;
    if(parent != null){
        var element = getElementSibling(parent, type);
        if(element != null){
            return  element;
        }
        else {
            return getParent(parent, type);
        }
    }
    // else {
    //     return getParent(parent);
    // }
}

function getElementSibling(parent, type){
    if(type == "next"){
        return parent.nextElementSibling;
    }
    else{
        return parent.previousElementSibling;
    }
}

//mengambil cell index
function getCellIndex(event){
    var parent = event.target.parentElement;
    if(parent.tagName.toLowerCase() == "td"){
        return parent.cellIndex;
    }
    else {
        var getparent = getParent(parent);
        return getparent.cellIndex;
    }
}

//mengambil element input atau textarea
function getInput(parent){
    var input = null;

    if(isInput(parent)) {
        input = parent.getElementsByTagName("input")[0];
    }
    else if(isTextarea(parent)){
        input = parent.getElementsByTagName("textarea")[0];
    }

    return input;
}

//kondisi jika ada element input
function isInput(event){
    if(event.getElementsByTagName("input").length > 0){
        return true;
    }else{
        return false;
    }
}

//kondisi jika ada element textarea
function isTextarea(event){
    if(event.getElementsByTagName("textarea").length > 0){
        return true;
    }else{
        return false;
    }
}

function arrow() {
    var keyCodes = {
        'up': 38,
        'down': 40,
        'left': 37,
        'right': 39,
        'backspace': 8
    }

    return keyCodes;
}

export default {
    arrownavigate,
}