﻿
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
	public partial class AnlGroupModelView : BaseForm {

		#region 定数

		#region 固定値、文字列
		/// <summary>
		/// 空き(表示用)
		/// </summary>
		const string FLAG_DISP = "";
		/// <summary>
		/// ゼロ値(表示用)
		/// </summary>
		const int FLAG_ZERO = 0;
		/// <summary>
		/// いち値(表示用)
		/// </summary>
		const string FLAG_ONE = "1";
		/// <summary>
		/// OK(表示用)
		/// </summary>
		const string FLAG_OK_DISP = "OK";
		/// <summary>
		/// NG(表示用)
		/// </summary>
		const string FLAG_NG_DISP = "NG";
		/// <summary>
		/// 画像解析項目構成一覧画面(表示用)
		/// </summary>
		const string FLAG_IMAGE_LIST_DISP = "型式紐づけグループ検索画面";
		/// <summary>
		/// オブジェクトダブルクリック
		/// </summary>
		const string ON_DBLCLICK = "ondblclick";
		/// <summary>
		/// メッセージ(Key)
		/// </summary>
		const string MSG_KEY = "MSG";
		/// <summary>
		/// 表示用リスト
		/// </summary>
		private Dictionary<string, string> dicDataList = new Dictionary<string, string>();
		/// <summary>
		/// 選択用リスト
		/// </summary>
		public static Dictionary<string, string> dicSelectedRow;
		/// <summary>
		/// △(ASC)
		/// </summary>
		const string ALLOW_UP = " △";
		/// <summary>
		/// ▼(DESC)
		/// </summary>
		const string ALLOW_DOWN = " ▼";

		#endregion

		#region 検索条件

		/// <summary>
		/// 検索条件
		/// </summary>
		public class CONDITION {
			/// <summary>
			/// ステーション
			/// </summary>
			public static readonly ControlDefine STATION = new ControlDefine( "ddlStation", "ステーション", "ddlStation", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// ライン
			/// </summary>
			public static readonly ControlDefine LINE_CD = new ControlDefine( "ddlLineCd", "ライン", "ddlLineCd", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 設備
			/// </summary>
			public static readonly ControlDefine SETUBI = new ControlDefine( "ddlSetubi", "設備", "ddlSetubi", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 画像解析グループコード
			/// </summary>
			public static readonly ControlDefine IMG_GROUP_CD = new ControlDefine( "txtImgGroupCd", "画像解析グループコード", "txtImgGroupCd", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 画像解析グループ名
			/// </summary>
			public static readonly ControlDefine IMG_GROUP_NM = new ControlDefine( "txtImgGroupNm", "画像解析グループ名", "txtImgGroupNm", ControlDefine.BindType.Both, typeof( string ) );
		}

		#endregion

		#endregion

		#region グリッドビュー定義

		/// <summary>
		/// グリッドビュー定義
		/// </summary>
		public class GRID_SEARCHDETAIL {
			/// <summary>
			/// ライン
			/// </summary>
			public static GridViewDefine LINE_NM = new GridViewDefine( "ライン", "LineNm", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// ステーション
			/// </summary>
			public static GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "StationNm", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 設備
			/// </summary>
			public static GridViewDefine SETUBI = new GridViewDefine( "設備", "Setubi", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
			/// <summary>
			/// 画像解析グループコード
			/// </summary>
			public static GridViewDefine GROUP_CD = new GridViewDefine( "画像解析グループコード", "GroupCd", typeof( string ), "", true, HorizontalAlign.Center, 200, true, true );
			/// <summary>
			/// 画像解析グループ名
			/// </summary>
			public static GridViewDefine GROUP_NM = new GridViewDefine( "画像解析グループ名", "GroupNm", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
			/// <summary>
			/// ラインコード
			/// </summary>
			public static GridViewDefine LINE_CD = new GridViewDefine( "ラインコード", "LineCd", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
			/// <summary>
			/// ステーションコード
			/// </summary>
			public static GridViewDefine STATION_CD = new GridViewDefine( "ステーションコード", "StationCd", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
			/// <summary>
			/// 設備名
			/// </summary>
			public static GridViewDefine SETUBI_NM = new GridViewDefine( "設備名", "SetubiNm", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
			/// <summary>
			/// 情報1
			/// </summary>
			public static GridViewDefine JYOHO1 = new GridViewDefine( "情報1", "Jyoho1", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報2
			/// </summary>
			public static GridViewDefine JYOHO2 = new GridViewDefine( "情報2", "Jyoho2", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報3
			/// </summary>
			public static GridViewDefine JYOHO3 = new GridViewDefine( "情報3", "Jyoho3", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報4
			/// </summary>
			public static GridViewDefine JYOHO4 = new GridViewDefine( "情報4", "Jyoho4", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報5
			/// </summary>
			public static GridViewDefine JYOHO5 = new GridViewDefine( "情報5", "Jyoho5", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報6
			/// </summary>
			public static GridViewDefine JYOHO6 = new GridViewDefine( "情報6", "Jyoho6", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報7
			/// </summary>
			public static GridViewDefine JYOHO7 = new GridViewDefine( "情報7", "Jyoho7", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報8
			/// </summary>
			public static GridViewDefine JYOHO8 = new GridViewDefine( "情報8", "Jyoho8", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報9
			/// </summary>
			public static GridViewDefine JYOHO9 = new GridViewDefine( "情報9", "Jyoho9", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報10
			/// </summary>
			public static GridViewDefine JYOHO10 = new GridViewDefine( "情報10", "Jyoho10", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報11
			/// </summary>
			public static GridViewDefine JYOHO11 = new GridViewDefine( "情報11", "Jyoho11", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報12
			/// </summary>
			public static GridViewDefine JYOHO12 = new GridViewDefine( "情報12", "Jyoho12", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報13
			/// </summary>
			public static GridViewDefine JYOHO13 = new GridViewDefine( "情報13", "Jyoho13", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報14
			/// </summary>
			public static GridViewDefine JYOHO14 = new GridViewDefine( "情報14", "Jyoho14", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報15
			/// </summary>
			public static GridViewDefine JYOHO15 = new GridViewDefine( "情報15", "Jyoho15", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報16
			/// </summary>
			public static GridViewDefine JYOHO16 = new GridViewDefine( "情報16", "Jyoho16", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報17
			/// </summary>
			public static GridViewDefine JYOHO17 = new GridViewDefine( "情報17", "Jyoho17", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報18
			/// </summary>
			public static GridViewDefine JYOHO18 = new GridViewDefine( "情報18", "Jyoho18", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報19
			/// </summary>
			public static GridViewDefine JYOHO19 = new GridViewDefine( "情報19", "Jyoho19", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報20
			/// </summary>
			public static GridViewDefine JYOHO20 = new GridViewDefine( "情報20", "Jyoho20", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報21
			/// </summary>
			public static GridViewDefine JYOHO21 = new GridViewDefine( "情報21", "Jyoho21", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報22
			/// </summary>
			public static GridViewDefine JYOHO22 = new GridViewDefine( "情報22", "Jyoho22", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報23
			/// </summary>
			public static GridViewDefine JYOHO23 = new GridViewDefine( "情報23", "Jyoho23", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報24
			/// </summary>
			public static GridViewDefine JYOHO24 = new GridViewDefine( "情報24", "Jyoho24", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報25
			/// </summary>
			public static GridViewDefine JYOHO25 = new GridViewDefine( "情報25", "Jyoho25", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報26
			/// </summary>
			public static GridViewDefine JYOHO26 = new GridViewDefine( "情報26", "Jyoho26", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報27
			/// </summary>
			public static GridViewDefine JYOHO27 = new GridViewDefine( "情報27", "Jyoho27", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報28
			/// </summary>
			public static GridViewDefine JYOHO28 = new GridViewDefine( "情報28", "Jyoho28", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 情報29
			/// </summary>
			public static GridViewDefine JYOHO29 = new GridViewDefine( "情報29", "Jyoho29", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
		}

		/// <summary>
		/// グリッドビューグループ
		/// </summary>
		const string GRID_ANLGROUP_SEARCH_VIEW_GROUP_CD = "AnlGroupModelView";

		#endregion

		#region プロパティ

		/// <summary>
		/// ロガー
		/// </summary>
		private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		/// <summary>
		/// 表示中ページ(ベースフォーム)情報
		/// </summary>
		private BaseForm CurrentForm {
			get {
				return ( (BaseForm)Page );
			}
		}

		/// <summary>
		/// 検索条件定義情報
		/// </summary>
		ControlDefine[] _conditionControls = null;

		/// <summary>
		/// 検索条件定義情報アクセサ
		/// </summary>
		ControlDefine[] ConditionControls {
			get {
				if ( true == ObjectUtils.IsNull( _conditionControls ) ) {
					_conditionControls = ControlUtils.GetControlDefineArray( typeof( CONDITION ) );
				}
				return _conditionControls;
			}
		}

		/// <summary>
		/// 一覧定義情報
		/// </summary>
		GridViewDefine[] _gridviewDefault = null;

		/// <summary>
		/// 一覧定義情報アクセサ
		/// </summary>
		GridViewDefine[] GridviewDefault {
			get {
				if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
					_gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHDETAIL ) );
				}
				return _gridviewDefault;
			}
		}

		#endregion

		#region イベント

		/// <summary>
		/// 画面ロード時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load( object sender, EventArgs e ) {
			base.RaiseEvent( DoPageLoad, false );
		}

		/// <summary>
		/// 検索ボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSearch_Click( object sender, EventArgs e ) {
			// 検索条件を作成する
			ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();
			base.RaiseEvent( DoSearch );
		}

		/// <summary>
		/// 閉じるボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnViewRefresh_Click( object sender, EventArgs e ) {
			base.RaiseEvent( DoSearch );
		}

		/// <summary>
		/// 画像解析構成ボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnModalDisp_Click( object sender, EventArgs e ) {
			dicDataList.Clear();
			dicDataList = ParseToDictionary( resultText.Text );
			DoImgAnl();
			//base.RaiseEvent( DoImgAnl );
			for ( int idx = 0; idx < grvDetailHeaderRT.Columns.Count; idx++ ) {
				string sValue = "";
				if ( false == dicDataList.TryGetValue( grvDetailHeaderRT.Columns[idx].SortExpression, out sValue ) ) {
					grvDetailHeaderRT.Columns[idx].Visible = false;
					grvDetailBodyRB.Columns[idx].Visible = false;
				}
			}
		}

		/// <summary>
		/// グリッドビューページチェンジイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvDetailBody_PageIndexChanging( object sender, CommandEventArgs e ) {
			base.RaiseEvent( PageIndexChanging, sender, e );
			RestoreMsg();
		}

		/// <summary>
		/// 一覧データバインドイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {

			try {
				ClearApplicationMessage();
				RowDataBound( sender, e );

			} catch ( Exception ex ) {
				//イベント処理中にエラー発生
				Logger.Exception( ex );
				throw ex;
			}
		}

		/// <summary>
		/// 一覧データバインドイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {

			try {
				ClearApplicationMessage();
				RowDataBound( sender, e );

			} catch ( Exception ex ) {
				//イベント処理中にエラー発生
				Logger.Exception( ex );
				throw ex;
			}
		}

		#endregion

		#region ページ処理

		/// <summary>
		/// ページロード処理
		/// </summary>
		protected override void DoPageLoad( ) {

			// ベースページロード処理
			base.DoPageLoad();

			// グリッドビューの再表示
			GridView frozenGrid = grvDetailBodyLB;
			ControlUtils.SetGridViewTemplateField( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
			SetGridViewTemplateField( grvDetailBodyRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) , resultText.Text);
			ControlUtils.ReMakeBoundTemplateFieldCtrl( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
			ControlUtils.ReMakeBoundTemplateFieldCtrl( grvDetailBodyRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
			int resultCnt = 0;

			if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
				//検索結果がnullでない場合
				resultCnt = ConditionInfo.ResultData.Rows.Count;
			}

			//ページャー設定
			ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBodyRB, grvDetailBody_PageIndexChanging, resultCnt, grvDetailBodyRB.PageIndex );

		}

		/// <summary>
		/// 初期化処理
		/// </summary>
		protected override void Initialize( ) {

			// アクセスカウンター登録
			Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

			// ベース処理初期化処理
			base.Initialize();

			// セッションをクリアする
			ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

			// 初期処理
			InitializeValues();
		}

		#endregion

		#region 機能別処理

		#region 初期処理

		/// <summary>
		/// 初期処理
		/// </summary>
		private void InitializeValues() {

			try {
				// システム検査日を検索条件：検査日の開始の初期値に設定する
				String systemDate = DateUtils.ToString( DateTime.Now, DateUtils.DATE_FORMAT_DAY );
				DateTime? defaultDate = DateUtils.ToDateNullable( systemDate, DateUtils.DATE_FORMAT_DAY );

				// ライン一覧情報を検索条件：ラインのリストに設定する
				ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectLineListByStation( ddlStation.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
				if ( 0 < ServiceLineList.Length ) {
					//検索結果がnullでない場合
					ddlLineCd.Items.Clear();
					ddlLineCd.Items.Add( FLAG_DISP );
					ddlLineCd.Items.AddRange( ServiceLineList );
				}

				// ステーション一覧情報を検索条件：ステーションのリストに設定する
				ListItem[] ServiceStationList = ControlUtils.GetListItems( AiImageCheckDao.SelectStationListByLine( ddlLineCd.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
				if ( 0 < ServiceStationList.Length ) {
					//検索結果がnullでない場合
					ddlStation.Items.Clear();
					ddlStation.Items.Add( FLAG_DISP );
					ddlStation.Items.AddRange( ServiceStationList );
				}

				// 設備一覧情報を検索条件：ステーションのリストに設定する
				ddlSetubi.Items.Clear();
				ddlSetubi.Items.Add( FLAG_DISP );

				// 件数変数の初期化
				ntbResultCount.Value = 0;

				// 表示領域の初期化
				divGrvDisplay.Visible = false;


			} catch ( Exception ex ) {
				// 例外ログ、メッセージ表示を実行する
				Logger.Exception( ex );
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
			}
		}

		/// <summary>
		/// ステーションリスト選択動作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlLineList_SelectedIndexChanged( object sender, EventArgs e ) {
			base.RaiseEvent( ChangeLineListByStation );
		}

		/// <summary>
		/// ステーション変更
		/// </summary>
		private void ChangeLineListByStation( ) {

			if ( true == StringUtils.IsNotEmpty( ddlStation.SelectedValue ) ) {
				ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectLineListByStation( ddlStation.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );

				if ( 0 < ServiceLineList.Length ) {
					//検索結果がnullでない場合
					ddlLineCd.SelectedValue = ServiceLineList[0].Value;
					ListItem[] ServiceSetubiList = ControlUtils.GetListItems( AiImageCheckDao.SelectSetubiByLineStation( ddlLineCd.SelectedValue, ddlStation.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
					ddlSetubi.Items.Clear();
					ddlSetubi.Items.Add( FLAG_DISP );

					if ( 0 < ServiceSetubiList.Length ) {
						//検索結果がnullでない場合s
						ddlSetubi.Items.AddRange( ServiceSetubiList );
						ddlSetubi.SelectedValue = ServiceSetubiList[0].Value;
					}
				}

			} else {
				ddlSetubi.Items.Clear();
				ddlSetubi.Items.Add( FLAG_DISP );
			}
		}

		/// <summary>
		/// ラインリスト選択動作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlStationList_SelectedIndexChanged( object sender, EventArgs e ) {
			if ( true == StringUtils.IsNotEmpty( ddlLineCd.SelectedValue ) ) {
				base.RaiseEvent( ChangeStationListByLine );
			}
		}

		/// <summary>
		/// ライン変更
		/// </summary>
		private void ChangeStationListByLine( ) {
			ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectStationListByLine( ddlLineCd.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );

			if ( 0 < ServiceLineList.Length ) {
				//検索結果がnullでない場合
				ddlStation.Items.Clear();
				ddlStation.Items.Add( FLAG_DISP );
				ddlStation.Items.AddRange( ServiceLineList );
				if (ServiceLineList.Length == 1)
				{
					ddlStation.SelectedValue = ServiceLineList[0].Value.ToString();
				}
			}
		}

		#endregion

		#region 型式紐づけ遷移前処理

		/// <summary>
		/// 型式紐づけ遷移前処理
		/// </summary>
		private void DoImgAnl( ) {

			//int a = grvDetailBodyLB.SelectedRow.RowIndex;

			// 必要な選択してを確認する
			if ( true == StringUtils.IsEmpty( txtComment.Text ) ) {

				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61080 );
				return;

			} else {

				// 辞書変数の初期化
				dicSelectedRow = ParseToDictionary( txtComment.Text );

				// アプリケーションメッセージクリア
				base.ClearApplicationMessage();

				// テキスト文字列の初期化
				txtComment.Text = "";
				ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "AnlGroupModelView_BthClick", "ControlCommon.DoubleClickGridRow(0,1,'AnlGroupModelList.aspx','" + base.Token + "');", true );
			}
		}

		#endregion

		#region 検索処理

		/// <summary>
		/// 検索処理
		/// </summary>
		private void DoSearch( ) {

			Dictionary<string, object> dicCondition = new Dictionary<string, object>();

			if ( ConditionInfo.conditionValue != null ) {
				dicCondition = ConditionInfo.conditionValue;

			} else {
				base.GetControlValues( ConditionControls, ref dicCondition );
			}

			// 必要な検索条件を確認する
			if ( true == StringUtils.IsEmpty( ddlLineCd.SelectedValue ) ) {

				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "ライン" );
				ClearGridView();
				return;

			} else if ( true == StringUtils.IsEmpty( ddlStation.SelectedValue ) ) {

				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "ステーション" );
				ClearGridView();
				return;

			} else if ( ddlSetubi.Items.Count < 2 ) {

				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_64020, "設備" );
				ClearGridView();
				return;

			};

			// 検索時画面情報を取得する
			Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
			base.GetControlTexts( ConditionControls, out dicIdWithText );

			// 検索結果取得
			// エラー時にも最後まで処理を通すため、インスタンスを生成しておく
			var result = new AiImageCheckBusiness.ImgCheckResultSet();
			DataTable tblResult = null;

			// 検索上限数を取得する
			int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;

			try {
				//throw new Exception();
				// 検索処理を実行する
				result = AiImageCheckBusiness.SelectGroupList( dicCondition, GridviewDefault, maxGridViewCount );

			} catch ( DataAccessException ex ) {

				if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
					// クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
					base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );

				} else {

					// タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
					Logger.Exception( ex );
					base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
				}
			} catch ( Exception ex ) {

				// Exceptionが発生した場合、エラーメッセージを設定する
				Logger.Exception( ex );
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );

			} finally {
			}

			//---検索結果判定---
			// 処理結果が取得できた時のみ件数・ページャーの設定を実施
			ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
			tblResult = result.ListTable;

			if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
				// 検索結果が存在する場合、件数表示、ページャーの設定を行う
				ntbResultCount.Value = tblResult.Rows.Count;

				// ピッキング明細件数に応じてページャー設定
				ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBodyRB, grvDetailBody_PageIndexChanging, tblResult.Rows.Count, 0 );

				// 検索条件/結果インスタンスを保持する
				cond.conditionValue = dicCondition;
				cond.IdWithText = dicIdWithText;
				cond.ResultData = tblResult.DefaultView.ToTable();

				//表示されている列名を動的に収集する
				if ( true == ObjectUtils.IsNull( dicDataList ) ) {
					dicDataList = new Dictionary<string, string>();
				} else {
					dicDataList.Clear();
				}
				if ( 0 < tblResult.Rows.Count ) {
					for ( int idx = FLAG_ZERO; idx < tblResult.Columns.Count; idx++ ) {
						if ( tblResult.Columns[idx].ColumnName.Contains( "Enable" ) ) {
							if ( tblResult.Rows[FLAG_ZERO].ItemArray[idx].ToString().Equals( FLAG_ONE ) ) {
								dicDataList.Add( "Jyoho" + tblResult.Columns[idx].ColumnName.Replace( "Enable", "" ), tblResult.Rows[FLAG_ZERO].ItemArray[idx - 1].ToString() );
							}
						}
					}
					resultText.Text = ParseToString( dicDataList );
				}

			} else {

				// タイムアウト等Exception時には、GridViewクリア
				ClearGridView();
			}

			// 検索条件をセッションに格納する
			ConditionInfo = cond;

			// グリッドビューの表示処理を行う
			GridView frozenGrid = grvDetailBodyLB;
			if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
				if ( 0 < tblResult.Rows.Count ) {

					//TemplateFieldの追加
					grvDetailHeaderRT.Columns.Clear();
					grvDetailBodyRB.Columns.Clear();
					for ( int idx = frozenGrid.Columns.Count; idx < GridviewDefault.Length; idx++ ) {
						TemplateField tf = new TemplateField();
						tf.HeaderText = StringUtils.ToString( GridviewDefault[idx].bindField );
						grvDetailBodyRB.Columns.Add( tf );
					}

					//新規バインド
					ControlUtils.ShowGridViewHeader( grvDetailHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
					ShowGridViewHeader( grvDetailHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo, true,resultText.Text );
					ControlUtils.BindGridView_WithTempField( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), tblResult );
					BindGridView_WithTempField( grvDetailBodyRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), tblResult,resultText.Text );

					for ( int idx = 0; idx < grvDetailHeaderRT.Columns.Count; idx++ ) {
						string sValue = "";
						if ( false == dicDataList.TryGetValue( grvDetailHeaderRT.Columns[idx].SortExpression, out sValue ) ) {
							grvDetailHeaderRT.Columns[idx].Visible = false;
							grvDetailBodyRB.Columns[idx].Visible = false;
						}
					}
					// 新規バインド
					// GridView
					divGrvDisplay.Visible = true;

					// グリッドビュー外のDivサイズ変更
					SetDivGridViewWidth();
			

					//テキスト文字列の初期化
					txtComment.Text = "";
				} else {

					//一覧初期化
					ClearGridView();
				}
			}

			//メッセージ表示
			if ( null != result.Message ) {
				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( result.Message );
			}
		}

		#endregion

		#region 一覧行データバインド

		/// <summary>
		/// 一覧行データバインド
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RowDataBound( params object[] parameters ) {
			var sender = (GridView)parameters[0];
			var e = (GridViewRowEventArgs)parameters[1];
			if ( e.Row.RowType == DataControlRowType.DataRow ) {

				// データ行のみバインド処理を行う
				//int index = 0;
				ControlUtils.GridViewRowBound( sender, e, GRID_ANLGROUP_SEARCH_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );

				//keyの作成
				Dictionary<string, string> dict = new Dictionary<string, string>();
				dict.Add( "lLineNm", "ライン" );
				dict.Add( "txtLineNm", ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.LINE_NM.bindField].ToString() );
				dict.Add( "lStationNm", "ステーション" );
				dict.Add( "txtStationNm", ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.STATION_NM.bindField].ToString() );
				dict.Add( "lSetubi", "設備" );
				dict.Add( "txtSetubi", ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.SETUBI.bindField].ToString() ) ;
				dict.Add( "lGroupCd", "画像解析グループコード" );
				dict.Add( "txtGroupCd", ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.GROUP_CD.bindField].ToString());
				dict.Add( "lGroupNm", "画像解析グループ名" );
				dict.Add( "txtGroupNm", ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.GROUP_NM.bindField].ToString());
				int idx = 1;
				dicDataList.Clear();
				dicDataList = ParseToDictionary( resultText.Text );
				foreach ( KeyValuePair<string, string> item in dicDataList ) {
					dict.Add( "lJyoho" + idx.ToString(), item.Value.ToString());
					dict.Add( "txtJyoho" + idx.ToString(), ( (DataRowView)e.Row.DataItem ).Row[item.Key].ToString() );
					idx++;
				}

				// クリック用
				e.Row.Attributes[ControlUtils.ON_CLICK] = e.Row.Attributes[ControlUtils.ON_CLICK] + " ";
				e.Row.Attributes[ControlUtils.ON_DBLCLICK] = e.Row.Attributes[ControlUtils.ON_CLICK] + "AnlGroupModelView.TxtWrite('" + ParseToString( dict ) + "');AnlGroupModelView.BthClick();";
				//e.Row.Attributes[ControlUtils.ON_DBLCLICK] = "return AnlGroupModelView.BthClick('" + base.Token + "');";
				//btnModalDispTmp.Attributes[ControlUtils.ON_CLICK] = "ControlCommon.DoubleClickGridRow(0,1,'AnlGroupModelList.aspx','" + base.Token + "');";
			}
		}

		#endregion

		#endregion

		#region グリッドビュー操作

		/// <summary>
		/// グリッドビュークリア
		/// </summary>
		private void ClearGridView( ) {

			// 列名非表示 グリッドビュークリア
			ControlUtils.InitializeGridView( grvDetailHeaderLT, false );
			ControlUtils.InitializeGridView( grvDetailHeaderRT, false );
			ControlUtils.InitializeGridView( grvDetailBodyLB, false );
			ControlUtils.InitializeGridView( grvDetailBodyRB, false );

			// 件数表示
			ntbResultCount.Value = 0;

			// ページャークリア
			ControlUtils.ClearPager( ref pnlPager );

			// GridView非表示
			divGrvDisplay.Visible = false;
			
		}

		/// <summary>
		/// グリッドビューページ切替
		/// </summary>
		/// <param name="parameters"></param>
		private void PageIndexChanging( params object[] parameters ) {
			object sender = parameters[0];
			CommandEventArgs e = (CommandEventArgs)parameters[1];
			int newPageIndex = Convert.ToInt32( e.CommandArgument );
			int pageSize = grvDetailBodyLB.PageSize;
			int rowCount = 0;
			if ( true == ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
				rowCount = ConditionInfo.ResultData.Rows.Count;
			}
			int allPages = 0;
			allPages = ConditionInfo.ResultData.Rows.Count / pageSize;
			if ( 0 != rowCount % pageSize ) {
				allPages += 1;
			}
			// ページが無くなっている場合には、先頭ページへ戻す
			if ( newPageIndex >= allPages ) {
				newPageIndex = 0;
			}

			//背面ユーザ切替対応
			ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
			GridView frozenGrid = grvDetailBodyLB;
			ControlUtils.ShowGridViewHeader( grvDetailHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
			ShowGridViewHeader( grvDetailHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true,resultText.Text );
			ControlUtils.BindGridView_WithTempField( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
			BindGridView_WithTempField( grvDetailBodyRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo.ResultData, resultText.Text );
			dicDataList.Clear();
			dicDataList = ParseToDictionary( resultText.Text );
			for ( int idx = 0; idx < grvDetailHeaderRT.Columns.Count; idx++ ) {
				string sValue = "";
				if ( false == dicDataList.TryGetValue( grvDetailHeaderRT.Columns[idx].SortExpression, out sValue ) ) {
					grvDetailHeaderRT.Columns[idx].Visible = false;
					grvDetailBodyRB.Columns[idx].Visible = false;
				}
			}
			ControlUtils.GridViewPageIndexChanging( grvDetailBodyLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
			ControlUtils.GridViewPageIndexChanging( grvDetailBodyRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
			ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBodyRB, grvDetailBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvDetailBodyRB.PageIndex );
			
			// グリッドビュー外のDivサイズ変更
			SetDivGridViewWidth();
		}

		/// <summary>
		/// グリッドビュー外のDivサイズ変更
		/// </summary>
		private void SetDivGridViewWidth( ) {
			SetDivGridViewWidth( grvDetailHeaderLT, divGrvHeaderLT );
			SetDivGridViewWidth( grvDetailHeaderRT, divGrvHeaderRT );
			SetDivGridViewWidth( grvDetailBodyLB, divGrvLB );
			SetDivGridViewWidth( grvDetailBodyRB, divGrvRB );
		}

		/// <summary>
		/// グリッドビュー外のDivサイズ変更
		/// </summary>
		private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {

			// セル幅補正 Padding4px + Border(片側) 1px
			const int CELL_PADDING = 4 * 2 + 1;

			// テーブル幅補正 Border(片側) 1px
			const int OUT_BORDER = 1;
			var visibleColumns = grv.Columns.Cast<DataControlField>().Where( x => x.Visible ).ToList();
			int sumWidth = NumericUtils.ToInt( visibleColumns.Sum( x => x.HeaderStyle.Width.Value ) )
								+ CELL_PADDING * visibleColumns.Count()
								+ ( visibleColumns.Any() ? OUT_BORDER : 0 );
			div.Style["width"] = $"{ sumWidth }px";
		}

		/// <summary>
		/// メッセージの復元
		/// </summary>
		private void RestoreMsg( ) {
			var msg = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MSG_KEY );
			if ( 0 < msg.Count ) {
				// メッセージが設定されていた場合、メッセージ表示
				base.WriteApplicationMessage( (Msg)msg[MSG_KEY] );
			}
		}

		/// <summary>
		/// グリッドビューデータヘッダ表示のみ
		/// </summary>
		/// <param name="grv">グリッドビューコントロール</param>
		internal static void ShowGridViewHeader( GridView grv, GridViewDefine[] gridViewDefineArr, ConditionInfoSessionHandler.ST_CONDITION cond, bool isSort, string resultText ) {

			InitializeGridView( grv, true );

			grv.AutoGenerateColumns = false;
			grv.ShowHeaderWhenEmpty = true;

			double sumWidth = 0;
			foreach ( GridViewDefine grvDef in gridViewDefineArr ) {
				string sValue = "";
				BoundField bf = new BoundField();
				bf.HtmlEncode = false;
				bf.HeaderText = grvDef.headerText;
				bf.HeaderStyle.Width = grvDef.width;
				Dictionary<string, string> dicDataListNew = new Dictionary<string, string>();
				dicDataListNew = ParseToDictionary( resultText);
				if ( dicDataListNew.TryGetValue( grvDef.bindField, out sValue ) ) {
					bf.HeaderText = sValue;
					if ( 0 < dicDataListNew.Count ) {
						if ( true == dicDataListNew.TryGetValue( grvDef.bindField.ToString(), out sValue ) ) {
							int iByteNum = System.Text.Encoding.Default.GetByteCount( sValue );
							bf.HeaderStyle.Width = iByteNum * 10;
						}
					}
				}
				bf.HeaderStyle.Height = 20;

				bf.Visible = grvDef.visible;

				if ( true == isSort && true == grvDef.sorting ) {
					bf.SortExpression = grvDef.sortingCol;
				}

				grv.Columns.Add( bf );

				if ( true == bf.Visible ) {
					sumWidth += bf.HeaderStyle.Width.Value;
				}
			}

			RenameSortedColumnNm( grv, cond );

			grv.HeaderStyle.Width = new Unit( sumWidth );
			grv.DataSource = new DataTable();
			grv.DataBind();
		}

		/// <summary>
		/// グリッドビューデータ初期化
		/// </summary>
		/// <param name="grv">グリッドビューコントロール</param>
		internal static void InitializeGridView( GridView grv, bool isClearColumns = false ) {
			grv.PageIndex = 0;
			grv.DataSource = null;
			if ( true == isClearColumns ) {
				grv.Columns.Clear();
			}
			grv.DataBind();
		}

		/// <summary>
		/// ソート列名 表示変更
		/// </summary>
		/// <param name="grv">グリッドビューコントロール</param>
		/// <param name="cond">グリッドビューバインド元データ格納構造体</param>
		private static void RenameSortedColumnNm( GridView grv, ConditionInfoSessionHandler.ST_CONDITION cond ) {

			foreach ( DataControlField column in grv.Columns ) {

				column.HeaderText = column.HeaderText.Replace( ALLOW_UP, "" );
				column.HeaderText = column.HeaderText.Replace( ALLOW_DOWN, "" );

				if ( column.SortExpression == cond.SortExpression ) {

					if ( cond.SortDirection == SortDirection.Ascending ) {
						column.HeaderText += ALLOW_UP;
					} else {
						column.HeaderText += ALLOW_DOWN;
					}
				}
			}
		}

		#region 解析する

		#region 辞書を文字列に解析
		/// <summary>
		/// 辞書を文字列に解析
		/// </summary>
		/// <param name="parameters">Dictionary</param>
		/// <returns>String</returns>
		public static string ParseToString( IDictionary<string, string> dem ) {

			StringBuilder query = new StringBuilder( "" );
			foreach ( KeyValuePair<string, string> item in dem ) {
				string key = item.Key;
				string value = item.Value;
				if ( false == string.IsNullOrEmpty( key )) {
					query.Append( key ).Append( "=" ).Append( value ).Append( '|' );
				}
			}
			string content = query.ToString().Substring( 0, query.Length - 1 );
			return content;
		}
		#endregion

		#region 辞書への文字列解析
		/// <summary>
		/// 辞書への文字列解析
		/// </summary>
		/// <param name="parameter">String</param>
		/// <returns>Dictionary</returns>
		public static Dictionary<string, string> ParseToDictionary( string parameter ) {
			try {
				String[] dataArry = parameter.Split( '|' );
				Dictionary<string, string> dataDic = new Dictionary<string, string>();
				for ( int i = 0; i <= dataArry.Length - 1; i++ ) {
					String dataParm = dataArry[i];
					int dIndex = dataParm.IndexOf( "=" );
					if ( dIndex != -1 ) {
						String key = dataParm.Substring( 0, dIndex );
						String value = dataParm.Substring( dIndex + 1, dataParm.Length - dIndex - 1 );
						dataDic.Add( key, value );
					}
				}

				return dataDic;
			} catch {
				return null;
			}
		}
		#endregion

		/// <summary>
		/// グリッドビューデータバインド
		/// </summary>
		/// <param name="grv">グリッドビューコントロール</param>
		/// <param name="gridViewColumnDef">グリッドビュー定義配列</param>
		/// <param name="tblData">バインド元データ</param>
		internal static void BindGridView_WithTempField( GridView grv, GridViewDefine[] gridViewColumnDef, DataTable tblData, string resultText ) {

			InitializeGridView( grv, false );
			grv.AutoGenerateColumns = false;

			int colIdx = 0;
			double sumWidth = 0;

			foreach ( GridViewDefine grvDef in gridViewColumnDef ) {

				int addBorderWidth = 1;
				if ( colIdx == 0 ) {
					addBorderWidth += 1;
				}

				if ( grv.Columns[colIdx] is BoundField ) {

					BoundField bf = (BoundField)grv.Columns[colIdx];
					bf.HeaderText = grvDef.headerText;

					bf.HeaderStyle.Width = grvDef.width;
					bf.HeaderStyle.Wrap = false;

					//バインドデータ及び列定義のフィールドがあればバインド
					if ( true == ObjectUtils.IsNotNull( tblData )
						&& 0 <= tblData.Columns.IndexOf( grvDef.bindField ) ) {

						bf.DataField = grvDef.bindField;
						bf.DataFormatString = grvDef.format;
						bf.ItemStyle.HorizontalAlign = grvDef.align;
						bf.ItemStyle.Wrap = false;
						bf.ItemStyle.Width = bf.HeaderStyle.Width;

						if ( true == grvDef.sorting ) {
							bf.SortExpression = grvDef.sortingCol;
						}
					}

					bf.Visible = grvDef.visible;

				} else if ( grv.Columns[colIdx] is TemplateField ) {

					TemplateField tf = (TemplateField)grv.Columns[colIdx];

					tf.HeaderText = grvDef.headerText;
					tf.HeaderStyle.Width = grvDef.width;

					string sValue = "";
					Dictionary<string, string> dicDataListNew = new Dictionary<string, string>();
					dicDataListNew = ParseToDictionary( resultText );
					if ( 0 <  dicDataListNew.Count ) {
						if ( true == dicDataListNew.TryGetValue( grvDef.bindField.ToString(), out sValue ) ) {
							int iByteNum = System.Text.Encoding.Default.GetByteCount( sValue );
							tf.HeaderStyle.Width = iByteNum * 10;
						}
					}

					tf.HeaderStyle.Wrap = false;
					tf.Visible = grvDef.visible;
					tf.ItemStyle.HorizontalAlign = grvDef.align;
					tf.ItemStyle.Width = tf.HeaderStyle.Width;

					if ( true == grvDef.sorting ) {
						tf.SortExpression = grvDef.sortingCol;
					}

					MakeBoundTemplateFieldCtrl( ref tf, grvDef ,resultText);
				} else if ( grv.Columns[colIdx] is ButtonField ) {

					ButtonField bf = (ButtonField)grv.Columns[colIdx];
					bf.HeaderText = grvDef.headerText;

					bf.HeaderStyle.Width = grvDef.width;
					bf.HeaderStyle.Wrap = false;

					//バインドデータ及び列定義のフィールドがあればバインド
					if ( true == ObjectUtils.IsNotNull( tblData )
						&& 0 <= tblData.Columns.IndexOf( grvDef.bindField ) ) {

						bf.DataTextField = grvDef.bindField;
						bf.DataTextFormatString = grvDef.format;
						bf.ItemStyle.HorizontalAlign = grvDef.align;
						bf.ItemStyle.Wrap = false;
						bf.ItemStyle.Width = bf.HeaderStyle.Width;

						if ( true == grvDef.sorting ) {
							bf.SortExpression = grvDef.sortingCol;
						}
					}

					bf.Visible = grvDef.visible;

				}

				if ( true == grv.Columns[colIdx].Visible ) {
					sumWidth += grv.Columns[colIdx].HeaderStyle.Width.Value;
				}

				colIdx++;

			}

			//grv.HeaderStyle.Width = new Unit( sumWidth );

			if ( true == ObjectUtils.IsNotNull( tblData ) ) {
				grv.DataSource = tblData.DefaultView;
				grv.DataBind();
			}

		}

		/// <summary>
		/// テンプレートフィールド用のバインドフィールド作成
		/// </summary>
		/// <param name="tmpField"></param>
		/// <param name="gridDef"></param>
		//internal static void MakeBoundTemplateFieldCtrl( ref TemplateField tmpField, GridViewDefine gridDef ) {
		internal static void MakeBoundTemplateFieldCtrl( ref TemplateField tmpField, GridViewDefine gridDef, string resultText ) {

			if ( false == gridDef.bindLabel ) {
				return;
			}

			string sValue = "";

			Dictionary<string, string> dicDataListNew = new Dictionary<string, string>();
			dicDataListNew = ParseToDictionary( resultText );
			if ( 0 < dicDataListNew.Count ) {
				if ( true == dicDataListNew.TryGetValue( gridDef.bindField.ToString(), out sValue ) ) {
					int iByteNum = System.Text.Encoding.Default.GetByteCount( sValue );
					gridDef.width = iByteNum * 10;
				}
			}

			LabelTemplateField itemTmp = new LabelTemplateField( gridDef );
			tmpField.ItemTemplate = itemTmp;

		}

		/// <summary>
		/// グリッドビューデータバインド
		/// </summary>
		/// <param name="grv">グリッドビューコントロール</param>
		/// <param name="gridViewColumnDef">グリッドビュー定義配列</param>
		/// <param name="tblData">バインド元データ</param>
		internal static void SetGridViewTemplateField( GridView grv, GridViewDefine[] gridViewColumnDef, string resultText ) {

			int colIdx = 0;

			foreach ( GridViewDefine grvDef in gridViewColumnDef ) {

				int addBorderWidth = 1;
				if ( colIdx == 0 ) {
					addBorderWidth += 1;
				}

				if ( grv.Columns[colIdx] is TemplateField ) {

					TemplateField tf = (TemplateField)grv.Columns[colIdx];
					tf.HeaderText = grvDef.headerText;

					tf.HeaderStyle.Width = grvDef.width;
					Dictionary<string, string> dicDataListNew = new Dictionary<string, string>();
					dicDataListNew = ParseToDictionary( resultText );
					if ( 0 < dicDataListNew.Count ) {
						string sValue = "";
						if ( true == dicDataListNew.TryGetValue( grvDef.bindField.ToString(), out sValue ) ) {
							int iByteNum = System.Text.Encoding.Default.GetByteCount( sValue );
							tf.HeaderStyle.Width = iByteNum * 10;
						}
					}

					tf.HeaderStyle.Wrap = false;
					tf.Visible = grvDef.visible;
					tf.ItemStyle.HorizontalAlign = grvDef.align;
					tf.ItemStyle.Width = tf.HeaderStyle.Width;

					if ( true == grvDef.sorting ) {
						tf.SortExpression = grvDef.sortingCol;
					}

					MakeBoundTemplateFieldCtrl( ref tf, grvDef,resultText );
				}

				colIdx++;

			}
		}

		#endregion

		#endregion
	}
}