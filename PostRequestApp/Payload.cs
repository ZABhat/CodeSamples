using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostRequestApp
{
    public class Campaign
    {
        public string campaignId { get; set; }
        public object incidentId { get; set; }
        public string isCustomerInterested { get; set; }
        public string customerReason { get; set; }
    }

    public class LaborLine
    {
        public string itemNumber { get; set; }
        public string itemName { get; set; }
        public string quantity { get; set; }
        public string uom { get; set; }
        public string billingCode { get; set; }
        public string laborRate { get; set; }
        public string billingType { get; set; }
        public string listPrice { get; set; }
        public string finalPrice { get; set; }
        public int lineId { get; set; }
        public int originalLineId { get; set; }
        public object lineNotes { get; set; }
    }

    public class PartLine
    {
        public string itemNumber { get; set; }
        public string itemName { get; set; }
        public string quantity { get; set; }
        public string uom { get; set; }
        public string billingCode { get; set; }
        public string billingType { get; set; }
        public string finalPrice { get; set; }
        public string listPrice { get; set; }
        public string partStatus { get; set; }
        public int lineId { get; set; }
        public int originalLineId { get; set; }
        public string inventoryId { get; set; }
        public int inventoryOrgId { get; set; }
        public string lineNotes { get; set; }
        public object attachments { get; set; }
        public bool pictureProvided { get; set; }
    }

    public class Payload
    {
        public List<PartLine> PartLines { get; set; }
        public List<LaborLine> LaborLines { get; set; }
        public List<object> ExpenseLines { get; set; }
        public int parentIncidentId { get; set; }
        public int incidentId { get; set; }
        public string activityId { get; set; }
        public string globalId { get; set; }
        public object skillLevel { get; set; }
        public object skillNeeded { get; set; }
        public string safetyCheckId { get; set; }
        public string activityStatus { get; set; }
        public object incompleteReason { get; set; }
        public object otherReason { get; set; }
        public string debriefCustName { get; set; }
        public string saf_additional_recipients { get; set; }
        public object numberTechNeeded { get; set; }
        public object pendingItemId { get; set; }
        public object vendorTypeId { get; set; }
        public object remainingTime { get; set; }
        public string debriefedProblemCode { get; set; }
        public string resolutionCode { get; set; }
        public string duration { get; set; }
        public object shift { get; set; }
        public bool deficiencyFlag { get; set; }
        public object technicianPreference { get; set; }
        public string customerPoNumber { get; set; }
        public object incompleteComments { get; set; }
        public bool quotedFlag { get; set; }
        public object srNumber { get; set; }
        public string debriefComments { get; set; }
        public object neededEquipment { get; set; }
        public int apptNumber { get; set; }
        public List<StatusLine> statusLines { get; set; }
        public object impersonatedBy { get; set; }
        public object nearMissReportId { get; set; }
        public List<object> Notes { get; set; }
        public string ParentIncidentCreateDate { get; set; }
        public string primaryClusteredReference { get; set; }
        public object clusteredDebriefFlag { get; set; }
        public object shippingOptions { get; set; }
        public object AVETestFlag { get; set; }
        public string AVETestNoReason { get; set; }
        public string Psr_Eligible { get; set; }
        public List<Campaign> Campaign { get; set; }
    }

    public class StatusLine
    {
        public int incidentId { get; set; }
        public DateTime adjustedTime { get; set; }
        public string activityStatus { get; set; }
        public DateTime systemTime { get; set; }
        public int apptNumber { get; set; }
    }
   
}
