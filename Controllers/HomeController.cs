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

using APIDemo.Helper;
using APIDemo.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace APIDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        ReturnResult _result;
        public HomeController()
        {
            _result = new ReturnResult();
        }
        //This gets called during the page load to get the user's information
        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<ReturnResult> GetUserInfo()
        {
            try
            {   
                //There is no authentication so userID is hard coded and passed
                //Usually in real world app, we get here only after authentication and getting userID
                string userID = "012345";
                var data = new Users();
                data = await ApiGatewayHelper<Users>.GetSingleItemRequest( "cardInfo", userID);
                _result.Success = true;
                _result.Data = data;
            }
            catch (Exception e)
            {
                _result.Success = false;
                _result.Message = "Error Occured While Processing Your Request. Please Contact System Administrator";
                _result.Data = e.StackTrace;
            }
            return _result;
        }


        //This makes the API call when toggling the card enable/disable
        [HttpGet]
        [Route("OnOff")]
        public async Task<ReturnResult> OnOffCard(string cardID)
        {
            try
            {
                _result = await ApiGatewayHelper<ReturnResult>.PostRequest("cardcontrols", "onoff/"+cardID,new ReturnResult(),new System.Threading.CancellationToken());
                _result.Success = true;
            }
            catch (Exception e)
            {
                _result.Success = false;
                _result.Message = "Error Occured While Processing Your Request. Please Contact System Administrator";
                _result.Data = e.StackTrace;
            }
            return _result;
        }

        //This makes API Post call when card is reported lost/damaged/stolen
        [HttpPost]
        [Route("PostRemarks")]
        public async Task<ReturnResult> PostCardRemarks([FromBody]CardRemarksInfo cardRemarksInfoObject)
        {
            try
            {
                _result = await ApiGatewayHelper<ReturnResult>.PostRequest("cardcontrols", "reportcardissue" ,
                    cardRemarksInfoObject, 
                    new System.Threading.CancellationToken());
                _result.Success = true;
            }
            catch (Exception e)
            {
                _result.Success = false;
                _result.Message = "Error Occured While Processing Your Request. Please Contact System Administrator";
                _result.Data = e.StackTrace;
            }
            return _result;
        }
    }
}
