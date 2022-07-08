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
 * If a card is reported lost/damaged/stolen then it should also be frozen. * 
 * 
 ********************************************************/


$(document).ready(function () {
    //Load the landing page
    loadDefaultData();
});

//This method is run at the page load and gets User/Credit Card information from the API
function loadDefaultData(){
    $.ajax({
        method: 'get',
        url: baseURL+'Home/GetUserInfo',
        contentType:  'application/json; charset=utf-8',
        //Makes call to the API and gets back response
        success: function (response) {
            if (response.success){
                var data = response.data;
                $('.lblUserName').html(data.cardHolder);

                var htmls = '';
                //Loop thorough  each card object that is returned, create the HTML and append it in the body
                $.each(data.cards,function(i,cardObject){
                    htmls += '<tr>';
                    htmls += '<td>' + cardObject.cardName+'</td>';
                    htmls += '<td>' + cardObject.maskedCardNumber+'</td>';
                    htmls +='<td>';
                    htmls +='<label class="switch">';
                    htmls += '    <input type="checkbox" class="chkCardStatus" data-key="' +cardObject.cardId+'" checked>';
                    htmls +='    <span class="slider round"></span>';
                    htmls +='  </label>';
                    htmls +='</td>';
                    htmls +='<td>';
                    htmls += '<button type="button" class="btnCustomAction actionButton" data-cardname="' + cardObject.cardName + '" data-key="' + cardObject.cardId+'"><i class="fa fa-envelope"></i></button>';
                    htmls +='</td>';
                    htmls += '</tr>';

                });
                $('.main-body').html(htmls);
            }else{
                showMessage('error', 'This message comes from API-' + response.message);
            }
        },
        error: function (e) { 
            showMessage('error','The system encountered an error -'+e );
            console.log(e);
         }
    });
}


//This function is triggered when the toggle switch is changed
$(document).on('change','.chkCardStatus',function(){
    var toggleSwitch = $(this);
    var proceed = false;
    var cardID = $(toggleSwitch).data('key');
    proceed = confirm('Are you sure to ' + ($(toggleSwitch).prop('checked')?"Activate/Unfreeze":"Deactivate/Freeze")+' this card?');
    //If user selects Yes, proceed. 
    if (proceed) {
        $.ajax({
            method: 'get',
            url: baseURL+'Home/OnOff?cardID='+cardID,
            contentType:  'application/json; charset=utf-8',
            success: function (response) {
                if (response.success){
                    showMessage('Success', 'This message comes from API - ' + response.message);
                }else{
                    showMessage('Error', 'This message comes from API - ' + response.message);
                }
            },
            error: function (e) { 
                showMessage('Error', 'The system has encountered an error - \n' + e);
                console.log(e);
             }
        });
    } else {
        //If user selects no, go back to original state
        $(toggleSwitch).prop('checked', !$(toggleSwitch).prop('checked'));
    }
});

//This event is triggered and function gets called when Remarks button is clicked
$(document).on('click','.btnCustomAction',function(){

    var btn = $(this);
    var cardID = $(btn).data('key');
    var cardName = $(btn).data('cardname');

    //Pass in Card id and name to the pop-up window
    $('#mdl_hdn_card_id').val(cardID);
    $('.mdl_lbl_cardName').html(cardName);

    //Open the pop-up Modal window
    openModal('myModal');
});
//This event is triggered when submit button is clicked on the pop-up window
$(document).on('click','.btnSaveCardStatus',function(){
    var cardObject = {
        cardId : $('#mdl_hdn_card_id').val() || '',
        cardStatus : $('#txtStatus').val() || '',
        comment: $('#txtComment').val() || '',
        cardName: $('.mdl_lbl_cardName').html() || ''
    }
    //Do a basic validation check that all fields are properly filled out
    if (!cardObject.cardId){
        showMessage('Error','Card ID Not Found');
        return;
    } else if (!cardObject.cardStatus){
        showMessage('Error','Select Card Status');
        return;
    } else if (!cardObject.comment){
        showMessage('Error','Enter Comment');
        return;
    } else {
        //If all fields are fine, make ajax call to API
        $.ajax({
            method: 'post',
            url: baseURL+'Home/PostRemarks',
            contentType:  'application/json; charset=utf-8',
            data: JSON.stringify(cardObject),
            success: function (response) {
                if (response.success){
                    showMessage('success','This message comes from API - '+ response.message);

                    //If card has been successfully reported/emailed and subject is not "General", deactivate the card
                    if (cardObject.cardStatus != 'general') { 
                        $('input[type=checkbox][data-key=' + cardObject.cardId + ']').prop('checked', false);
                        showMessage('Info', 'Your  ' + cardObject.cardName +' has been disabled!');
                    }
                    
                    closeModal('myModal');
                }else{
                    showMessage('Error', 'This message comes from API- '+response.message);
                }
            },
            error: function (e) { 
                showMessage('Error','The system has encountered an error '+ e);
                console.log(e);
             }
        });
    }
});