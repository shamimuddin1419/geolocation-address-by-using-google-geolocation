using GoogleGeolocationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace GoogleGeolocationApp.Controllers
{
    public class TaskController : ApiController
    {
        SalesTrackingEntities _dbContext = null;

        // constructor 
        public TaskController()
        {
            _dbContext = new SalesTrackingEntities();
        }
        // for Task save
        [HttpPost]
        public ResponseModel SaveTask(vmTask _vmTask)
        {
            ResponseModel _modelObj = new ResponseModel();
            int result = SaveTaskDetails(_vmTask);
            if (result == 1)
            {
                _modelObj.Status = true;
                _modelObj.Message = "Task Saved";
            }
            else
            {
                _modelObj.Status = false;
                _modelObj.Message = "Task Saved faill";
            }
            return _modelObj;
        }

        [HttpGet]
        // for get Task data by UserID
        public ResponseModel GetTaskList(Int64 userID)
        {
            ResponseModel _modelObj = new ResponseModel();
            List<vmTask> taskes = GetTaskDetails(userID);
            if (taskes.Count > 0)
            {
                _modelObj.Data = taskes;
                _modelObj.Status = true;
                _modelObj.Message = "Data get successfully.";
            }
            else
            {
                _modelObj.Data = taskes;
                _modelObj.Status = false;
                _modelObj.Message = "Data not found.";
            }
            return _modelObj;
        }
        private List<vmTask> GetTaskDetails(long userID)
        {
            List<vmTask> taskes = null;
            try
            {
                taskes = (from ds in _dbContext.crm_DailySalesTask
                          where ds.UserID == userID
                          select new vmTask
                          {
                              UserID = ds.UserID ?? 0,
                              BuyerName = ds.BuyerName,
                              Achivement = ds.Achivement,
                              Description = ds.Description,
                              Address = ds.Address
                          }).ToList();
                return taskes;
            }
            catch
            {
                taskes = null;

            }
            return taskes;
        }
        private int SaveTaskDetails(vmTask _vmTask)
        {
            crm_DailySalesTask _objSalesTask = new crm_DailySalesTask();
            int result = 0;
            string Address = "";
            try
            {
                Int64 nextRow = _dbContext.crm_DailySalesTask.Count() + 1;
                _objSalesTask.DailySalesTaskID = nextRow;
                _objSalesTask.UserID = _vmTask.UserID;
                _objSalesTask.BuyerName = _vmTask.BuyerName;
                _objSalesTask.Description = _vmTask.Description;
                _objSalesTask.Achivement = _vmTask.Achivement;
                _objSalesTask.Latitude = _vmTask.Latitude;
                _objSalesTask.Longitude = _vmTask.Longitude;
                Address = GetAddress(_objSalesTask.Latitude, _objSalesTask.Longitude);
                _objSalesTask.Address = Address;
                _dbContext.crm_DailySalesTask.Add(_objSalesTask);
                _dbContext.SaveChanges();
                result = 1;
            }
            catch
            {
                result = 0;

            }
            return result;
        }
        // get Address
        private string GetAddress(string latitude, string longitude)
        {
            string locationName = "";
            string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false", latitude, longitude);
            XElement xml = XElement.Load(url);
            if (xml.Element("status").Value == "OK")
            {
                locationName = string.Format("{0}",
                    xml.Element("result").Element("formatted_address").Value);
            }
            return locationName;
        }
    }
}
