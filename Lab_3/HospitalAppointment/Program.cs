public enum ReceptionHours { eight, nine, ten, eleven, twelve, fourteen, fifteen, sixteen, seventeen}
public enum Doctors {dentist, surgeon, urologist, neurologist, otolaryngologist, psychiatrist, pediatrician, ophthalmologist, gynecologist};
public struct ListAppointmentsOutput
{
    public bool searchResult;
    public List<IAppointment> appointments;
}
public struct ListTimesOutput
{
    public bool searchResult;
    public List<ReceptionHours> times;
}
public interface IAppointment
{
    public string GetName();
    public DateTime GetDate();
    public ReceptionHours GetTime();
    public Doctors GetDoctor();
}  
public class Equeue   
{   
    static void Main()
    {
        Console.WriteLine("Hey");
    }
    public bool AddAppointment(IAppointment client)
    {
        bool result = true;
        IAppointment? found = this.allAppointments.Find(item => ((item.GetDate == client.GetDate)
                                                                & (item.GetTime == client.GetTime)
                                                                & (item.GetDoctor == client.GetDoctor)));
        if (found == null)
        {
            this.allAppointments.Add(client);
            this.allAppointmentsCount++;
        }
        else
        {
            result = false;
        }
        return result;
    }
    public bool DeleteAppointment(IAppointment client)
    {
        bool result = this.allAppointments.Remove(client);
        if (result)
        {
            allAppointmentsCount--;
        }
        return result;
    }
    public ListAppointmentsOutput GetAppointmentsByName(string clientName)
    {
        List<IAppointment> currentNameAppointments = new List<IAppointment>();
        string currentName;
        foreach (IAppointment appointment in this.allAppointments)
        {
            currentName = appointment.GetName();
            if (currentName == clientName)
            {
                currentNameAppointments.Add(appointment);
            }
        }
        ListAppointmentsOutput result = new ListAppointmentsOutput();
        result.appointments = currentNameAppointments;
        if (currentNameAppointments.Count > 0)
        {
            result.searchResult = true;
        }
        else
        {
            result.searchResult = false;
        }
        return result;
    }
    public ListAppointmentsOutput GetAppointmentsByDoctorAndDate(Doctors doctor, DateTime requiredDate)
    {
        List<IAppointment> currentDoctorAndDateAppointments = new List<IAppointment>();
        Doctors currentDoctor;
        DateTime currentDate;
        foreach (IAppointment appointment in this.allAppointments)
        {
            currentDoctor = appointment.GetDoctor();
            currentDate = appointment.GetDate();
            if ((currentDoctor == doctor) && (currentDate == requiredDate))
            {
                currentDoctorAndDateAppointments.Add(appointment);
            }
        }
        ListAppointmentsOutput result = new ListAppointmentsOutput();
        result.appointments = currentDoctorAndDateAppointments;
        if (currentDoctorAndDateAppointments.Count > 0)
        {
            result.searchResult = true;
        }
        else
        {
            result.searchResult = false;
        }
        return result;
    }
    public ListAppointmentsOutput GetAppointmentsByDate(DateTime requiredDate)
    {
        List<IAppointment> currentDateAppointments = new List<IAppointment>();
        DateTime currentDate;
        foreach (IAppointment appointment in this.allAppointments)
        {
            currentDate = appointment.GetDate();
            if (currentDate == requiredDate)
            {
                currentDateAppointments.Add(appointment);
            }
        }
        ListAppointmentsOutput result = new ListAppointmentsOutput();
        result.appointments = currentDateAppointments;
        if (currentDateAppointments.Count > 0)
        {
            result.searchResult = true;
        }
        else
        {
            result.searchResult = false;
        }
        return result;
    }
    public ListTimesOutput GetFreeTimesByDoctorAndDate(Doctors doctor, DateTime requiredDate)
    {
        List<IAppointment> currentDateAppointments = GetAppointmentsByDoctorAndDate(doctor, requiredDate).appointments;
        List<ReceptionHours> freeTimes = new List<ReceptionHours>();
        ReceptionHours workTime;
        Doctors currentDoctor;
        DateTime currentDate;
        for (workTime = ReceptionHours.eight; workTime <= ReceptionHours.seventeen; workTime++)
        {
            freeTimes.Add(workTime);
        }
        foreach (IAppointment appointment in currentDateAppointments)
        {
            currentDoctor = appointment.GetDoctor();
            currentDate = appointment.GetDate();
            if ((currentDoctor == doctor) && (currentDate == requiredDate))
            {
                workTime = appointment.GetTime();
                if (freeTimes.Contains(workTime))
                {
                    freeTimes.Remove(workTime);
                }
            }       
        }
        ListTimesOutput result = new ListTimesOutput();
        result.times = freeTimes;
        if (currentDateAppointments.Count > 0)
        {
            result.searchResult = true;
        }
        else
        {
            result.searchResult = false;
        }
        return result;
    }
    private List<IAppointment> allAppointments = new List<IAppointment>();
    private int allAppointmentsCount = 0;
}

