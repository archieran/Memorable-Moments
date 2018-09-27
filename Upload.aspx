<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="CoreContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="fb-root"></div>
    <script>(function(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_GB/sdk.js#xfbml=1&version=v2.8&appId=165251097262664";
    fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));</script>
    

    <asp:Label ID="LabelDate" runat="server" CssClass="col-md-2 control-label"></asp:Label><br />
    <br />
    <div class="form-horizontal">
        
        <asp:Label ID="LabelText" runat="server" Text="My Thoughts" CssClass="col-md-2 control-label"></asp:Label>
        <br />
        <br />
        <asp:TextBox ID="TextBoxThought" runat="server" Height="300px" TextMode="MultiLine" CssClass="form-control" Width="346px"></asp:TextBox>
        
        <asp:FileUpload ID="FileUpload1" runat="server" style="position:absolute;left:500px;top:110px;" CssClass="btn btn-default"/>
        <asp:Button ID="ButtonUpload" runat="server" style="position:absolute;left:800px;top:110px;" Text="Upload" CssClass="btn btn-default" OnClick="ButtonUpload_Click" />
        <asp:Image ID="Image1" runat="server" style="position:absolute;left:800px;top:130px;"/>

        <br />
        <asp:Button ID="ButtonUploadThought" runat="server" Text="Upload Thought" CssClass="btn btn-default" OnClick="ButtonUploadThought_Click" />
        <br />
        <br />
        <div class="fb-share-button" data-href="http://localhost:58035/Upload.aspx" data-layout="button" data-size="large" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Flocalhost%3A58035%2FUpload.aspx&amp;src=sdkpreparse">Share</a></div>
    </div>
    
</asp:Content>

