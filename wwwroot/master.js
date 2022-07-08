/*********************************************************
 * 
 * Author : Ujjwal R. Poudel
 * Date:    July 7th 2022
 * Project: Coastal Credit Job Interview Mini Project
 * 
 * Description of the project : 
 * Create a single-page mobile-first web application that allows the user to control their credit card.  
 * The application will allow the user to lock/freeze their card to disable transactions or unlock/unfreeze it to enable transactions again.  
 * The current state of the card should be maintained within the “session”.  The user should get a confirmation that their submission was successful.  
 * The user should be able to submit messages or report an issue with their card (i.e. lost/damaged/stolen).  
 * If a card is reported lost/damaged/stolen then it should also be frozen.
 * 
 * 
 ********************************************************/
//Base Url 
var baseURL = 'https://localhost:44307/';

//Alert Box
function showMessage(type, msg){
    alert(type+' : '+msg);
}

//Pop-up Modal window Show
function openModal(mdl){
    $('#'+mdl).css('display','block');
}

//Pop-up Modal window Hide
function closeModal(mdl){
    clearModalField();
    $('#'+mdl).css('display','none');
}

//Clear the fields out when closing the pop-up window
function clearModalField(){
    $('#mdl_hdn_card_id').val('');
    $('#txtStatus').val('');
    $('#txtComment').val('');
}