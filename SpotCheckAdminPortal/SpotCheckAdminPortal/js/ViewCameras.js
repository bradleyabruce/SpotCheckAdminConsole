//properties

var startX;
var startY;

var lastTempRect = null;

var finalX;
var finalY;

var tempX;
var tempY;

var currentX;
var currentY;

var boxCoordinates = new Array();

var currentDeviceID;

//"Constructor"

document.addEventListener("readystatechange", addEventListeners);

function addEventListeners() {
    //Only attempt to add event listeners after the page is fully loaded

    if (document.readyState == "complete") {

        var addSpotButton = document.getElementById("addParkingSpotCoordinateButton");
        var removeSingleButton = document.getElementById("removeParkingSpotCoordinateButton");
        var removeAllButton = document.getElementById("removeAllParkingSpotCoordinatesButton");

        addSpotButton.addEventListener("click", addSpotButtonClick);
        removeSingleButton.addEventListener("click", removeSingleClick);
        removeAllButton.addEventListener("click", removeAllClick);

        var btnDeployCloseFooter = document.getElementById("deployCloseFooter");
        var btnDeployCloseHeader = document.getElementById("deployCloseHeader");

        btnDeployCloseFooter.addEventListener("click", deployClosed);
        btnDeployCloseHeader.addEventListener("click", deployClosed);

    }
}

//Events

function deployClosed()
{
    ResetAll();
}

function ResetAll()
{
    currentDeviceID = null;
    boxCoordinates = new Array();

    startX = null;
    startY = null;
    lastTempRect = null;
    finalX = null;
    finalY = null;
    tempX = null;
    tempY = null;
    currentX;
    currentY;
    clearCanvas();
}

function removeSingleClick(e) {
    //we need to add an event listener to the canvas, and if we are hovered over the coordinates of an existing spot. Allow the user to click it and remove it

    document.body.style.cursor = "crosshair";
    var imageCanvas = document.getElementById("imageCanvas");
    imageCanvas.addEventListener("mousedown", getMouseDownRemovePosition);
    imageCanvas.addEventListener("mousemove", getMouseMoveRemovePosition);

    var doneButton = document.getElementById("deployDone");
    doneButton.style = "visibility: visible;";
    doneButton.addEventListener("click", doneDeleting);

    document.getElementById("addParkingSpotCoordinateButton").disabled = true;
    document.getElementById("removeAllParkingSpotCoordinatesButton").disabled = false;

}

function doneDeleting()
{
    var doneButton = document.getElementById("deployDone");
    doneButton.style = "visibility: hidden;";
    doneButton.removeEventListener("click", doneDeleting);

    document.getElementById("addParkingSpotCoordinateButton").disabled = false;
    document.getElementById("removeAllParkingSpotCoordinatesButton").disabled = false;
    document.getElementById("removeParkingSpotCoordinateButton").disabled = false;S

    //clean up
    var imageCanvas = document.getElementById("imageCanvas");
    imageCanvas.removeEventListener("mousemove", getMouseMoveRemovePosition);
    imageCanvas.removeEventListener("mousedown", getMouseDownRemovePosition);
    document.body.style.cursor = "default";
}

function getMouseMoveRemovePosition(e) {
    //get current mouse Coordinates
    currentX = getCurrentX(e)
    currentY = getCurrentY(e)

    boxCoordinates.forEach(markForDelete);
    clearCanvas();
    boxCoordinates.forEach(drawAllRects);
}

function getCurrentX(e) {
    //Due to the fact that our canvas in within a series of parent divs, we need to account for the parents offset values to get the actual coordinates of a user click within the canvas
    //There is no way in hell I am testing this in other browsers
    var xoffset = 0;

    var obj1 = imageCanvas.parentElement;
    xoffset += obj1.offsetLeft;

    var obj2 = obj1.parentElement;
    var obj3 = obj2.parentElement;
    var obj4 = obj3.parentElement;
    xoffset += obj4.offsetLeft + 1;

    currentX = e.clientX - xoffset;
    return currentX;
}

function getCurrentY(e) {
    //Due to the fact that our canvas in within a series of parent divs, we need to account for the parents offset values to get the actual coordinates of a user click within the canvas
    //There is no way in hell I am testing this in other browsers
    var yoffset = 0;

    var obj1 = imageCanvas.parentElement;
    yoffset += obj1.offsetTop

    var obj2 = obj1.parentElement;
    yoffset += obj2.offsetTop

    var obj3 = obj2.parentElement;
    yoffset += obj3.offsetTop

    var obj4 = obj3.parentElement;
    yoffset += ((obj4.offsetTop * 2) - 3)

    currentY = e.clientY - yoffset;
    return currentY
}

function getMouseDownRemovePosition(e) {
    var filteredArray = boxCoordinates.filter(function (value, index, arr) {
        return value.hoveredForDelete == false;
    });

    if (filteredArray.length == boxCoordinates.length)   //the user has not clicked on an existing box
    {

    }
    else {
        boxCoordinates = filteredArray;

        clearCanvas();
        boxCoordinates.forEach(drawAllRects);
    }
}


function markForDelete(item, index) {
    //determine if the current coordinates are within the values of an existing box

    //if it lies on the top x plane
    if (currentY >= item.tlY && (currentX >= item.tlX && currentX <= (item.tlX + item.w))) {
        item.hoveredForDelete = true;
    }
    //if it lies on the bottom x plane
    //else if (currentY == (item.tlY + item.h) && (currentX >= item.tlX && currentX <= (item.tlX + item.w))) {
    //    item.hoveredForDelete = true;
    //}
    //if it lies on the left y plane
    //else if (currentX == item.tlX && (currentY >= item.tlY && currentY <= (item.tlY + item.h))) {
    //    item.hoveredForDelete = true;
    //}
    //if it lies on the right y plane
    else if (currentX <= (item.tlX + item.w) && (currentY >= item.tlY && currentY <= (item.tlY + item.h))) {
        item.hoveredForDelete = true;
    }
    else {
        item.hoveredForDelete = false;
    }
}

function removeAllClick(e) {
    //remove events from other buttons
    var imageCanvas = document.getElementById("imageCanvas");
    imageCanvas.removeEventListener("mousemove", getMouseMoveRemovePosition);
    imageCanvas.removeEventListener("mousedown", getMouseDownAddPosition);
    imageCanvas.removeEventListener("mouseup", getMouseUpAddPosition);
    imageCanvas.removeEventListener("mousedown", getMouseDownRemovePosition);
    document.body.style.cursor = "default";

    boxCoordinates = new Array();
    clearCanvas();
}

function addSpotButtonClick(e) {
    //remove events from other buttons
    var imageCanvas = document.getElementById("imageCanvas");

    document.body.style.cursor = "crosshair";

    imageCanvas.addEventListener("mousedown", getMouseDownAddPosition);
    imageCanvas.addEventListener("mouseup", getMouseUpAddPosition);

    var doneButton = document.getElementById("deployDone");
    doneButton.style = "visibility: visible;";
    doneButton.addEventListener("click", doneAdding);

    document.getElementById("removeParkingSpotCoordinateButton").disabled = true;
    document.getElementById("removeAllParkingSpotCoordinatesButton").disabled = true;
}

function doneAdding()
{
    imageCanvas.removeEventListener("mousemove", getMouseMoveRemovePosition);
    imageCanvas.removeEventListener("mousedown", getMouseDownRemovePosition);

    imageCanvas.removeEventListener("mousedown", getMouseDownAddPosition);
    imageCanvas.removeEventListener("mouseup", getMouseUpAddPosition);

    document.body.style.cursor = "default";

    document.getElementById("removeParkingSpotCoordinateButton").disabled = false;
    document.getElementById("removeAllParkingSpotCoordinatesButton").disabled = false;
}


function getMouseDownAddPosition(e) {

    imageCanvas = document.getElementById("imageCanvas");

    startX = getCurrentX(e);
    startY = getCurrentY(e);

    //we need to add an event handler to start listening for mouse movement
    imageCanvas.addEventListener("mousemove", getMouseMoveAddPosition);
}

function getMouseUpAddPosition(e) {

    imageCanvas = document.getElementById("imageCanvas");

    finalX = getCurrentX(e);
    finalY = getCurrentY(e);

    //we can now remove our event listener on mouse movement, down, and up and reset the cursor
    imageCanvas.removeEventListener("mousemove", getMouseMoveAddPosition);
    //


    //save our final box to our array and clean up the canvas
    addFinalBox();
    clearCanvas();
    boxCoordinates.forEach(drawAllRects);
}

function getMouseMoveAddPosition(e) {
    imageCanvas = document.getElementById("imageCanvas");

    tempX = getCurrentX(e);
    tempY = getCurrentY(e);

    drawTempBox();
}

function addFinalBox() {
    var topLeftX;
    var topLeftY;
    var bottomRightX;
    var bottomRightY;
    var height;
    var width;
    imageCanvas = document.getElementById("imageCanvas");

    //determine the topLeftCorner of the box, the bottomRight corner of the box, the height and width

    if (startX < finalX) {
        //start is to the left
        if (startY > finalY) {
            //start is bottom left
            topLeftX = startX;
            topLeftY = finalY;
        }
        else {
            //start is top left
            topLeftX = startX;
            topLeftY = startY;
        }
    }
    else {
        //start is to the right
        if (finalY > startY) {
            //final is the bottom left
            topLeftX = finalX;
            topLeftY = startY;
        }
        else {
            //final is the top left
            topLeftX = finalX;
            topLeftY = finalY;
        }
    }

    width = Math.abs(startX - finalX);
    height = Math.abs(startY - finalY);

    newRect =
    {
        tlX: topLeftX,
        tlY: topLeftY,
        w: width,
        h: height,
        hoveredForDelete: false
    };

    boxCoordinates.push(newRect);

    var ctx = imageCanvas.getContext("2d");
    ctx.strokeStyle = 'green';
    ctx.beginPath();
    ctx.rect(topLeftX, topLeftY, width, height);
    ctx.stroke();
}

function drawAllRects(item, index) {
    if (item.hoveredForDelete == false) {
        imageCanvas = document.getElementById("imageCanvas");
        var ctx = imageCanvas.getContext("2d");
        ctx.strokeStyle = 'green';
        ctx.beginPath();
        ctx.rect(item.tlX, item.tlY, item.w, item.h);
        ctx.stroke();
    }
    else if (item.hoveredForDelete == true) {
        imageCanvas = document.getElementById("imageCanvas");
        var ctx = imageCanvas.getContext("2d");
        ctx.strokeStyle = 'red';
        ctx.beginPath();
        ctx.rect(item.tlX, item.tlY, item.w, item.h);
        ctx.stroke();
    }
    else {

    }
}

function clearCanvas() {
    imageCanvas = document.getElementById("imageCanvas");
    var ctx = imageCanvas.getContext("2d");
    ctx.fillStyle = 'white';
    ctx.strokeStyle = 'black';
    ctx.beginPath();
    ctx.fillRect(0, 0, imageCanvas.width, imageCanvas.height);
    ctx.stroke();
}

function drawTempBox() {

    imageCanvas = document.getElementById("imageCanvas");

    //clear our canvas of all shit
    clearCanvas();

    //re-add the real boxes
    boxCoordinates.forEach(drawAllRects);

    //add our temp fake box

    if (startX < tempX) {
        //start is to the left
        if (startY > tempY) {
            //start is bottom left
            topLeftX = startX;
            topLeftY = tempY;
        }
        else {
            //start is top left
            topLeftX = startX;
            topLeftY = startY;
        }
    }
    else {
        //start is to the right
        if (tempY > startY) {
            //final is the bottom left
            topLeftX = tempX;
            topLeftY = startY;
        }
        else {
            //final is the top left
            topLeftX = tempX;
            topLeftY = tempY;
        }
    }

    width = Math.abs(startX - tempX);
    height = Math.abs(startY - tempY);

    var ctx = imageCanvas.getContext("2d");
    ctx.strokeStyle = 'black';
    ctx.beginPath();
    ctx.rect(topLeftX, topLeftY, width, height);
    ctx.stroke();
}