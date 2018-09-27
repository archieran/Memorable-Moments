<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Timeline.aspx.cs" Inherits="CoreContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">	 
    
    <br />
    <asp:Button ID="ButtonUpload" runat="server" Text="Upload New Thought" CssClass="btn btn-default" OnClick="ButtonUpload_Click" />
    
    <br /><br />
    <div class="form-horizontal">
        
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

    </div>
</asp:Content>

