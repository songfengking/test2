﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Pages.AiImageCheck;

namespace TRC_W_PWT_ProductView.Business {
	/// <summary>
	/// ビジネスクラス
	/// </summary>
	public class AiImageCheckBusiness {

		#region 定数

		#region 画像解析項目マスタ
		/// <summary>利用ツールプルダウンタイプ(検索用)</summary>
		private const string PULLDOWNTYPE_USINGTOOL = "3";
		#endregion

		#endregion

		#region 画像解析一覧情報格納構造体

		/// <summary>
		/// 画像解析一覧情報格納構造体
		/// </summary>
		[Serializable]
		public struct ImgCheckResultSet {
			/// <summary>メイン情報</summary>
			public DataTable ListTable { get; set; }
			/// <summary>サブ情報</summary>
			public Msg Message { get; set; }
		}

		#endregion

		#region 型変換

		/// <summary>
		/// 99999-99999形式の型式コードを大文字変換し9999999999形式にして返す
		/// </summary>
		///  <param name="txtCode">99999-99999形式の型式コード</param>
		/// <returns>処理結果</returns>
		public static String getLargeTextChange( String txtCode ) {
			if ( StringUtils.IsBlank( txtCode ) ) {
				return "";
			}
			txtCode = txtCode.Replace( "-", "" );
			return txtCode.ToUpper();
		}

		#endregion

		#region 検査画像ファイルパス取得処理
		/// <summary>
		/// 検査画像ファイルパス取得処理
		/// </summary>
		/// <param name="ddlStation">ステーション</param>
		/// <param name="txtIdno">IDNO</param>
		/// <param name="txtMonthlySeqNum">月度連番s</param>
		/// <param name="txtRecordNo">来歴</param>
		public static ImgCheckResultSet SearchOfImagePath( string ddlStation, string txtIdno, string txtMonthlySeqNum, string txtRecordNo) {
			//変数定義
			var result = new ImgCheckResultSet();

			//月度連番の大文字変換とハイフン削除
			if ( true == StringUtils.IsNotEmpty( txtMonthlySeqNum ) ) {
				txtMonthlySeqNum = getLargeTextChange( txtMonthlySeqNum );
			}

			//検索を実行する
			result.ListTable = AiImageCheckDao.SelectSearchImagePath( ddlStation, txtIdno, txtMonthlySeqNum, txtRecordNo, ref result );
			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 検査結果取得処理
		/// <summary>
		/// 検査結果取得処理
		/// </summary>
		/// <param name="ddlLineCd">ラインコード</param>
		/// <param name="ddlStation">ステーション</param>
		/// <param name="txtIdno">IDNO</param>
		/// <param name="txtMonthlySeqNum">月度連番s</param>
		/// <param name="txtRecordNo">来歴</param>
		/// <param name="ngFlag">不良タイプフラグ</param>
		/// <param name="correctiveFlag">是正フラグ</param>
		public static ImgCheckResultSet SearchOfImageAnlResultList( string ddlLineCd, string ddlStation, string txtIdno, string txtMonthlySeqNum, string txtRecordNo, string ngFlag, string correctiveFlag ) {
			//変数定義
			var result = new ImgCheckResultSet();

			//月度連番の大文字変換とハイフン削除
			if ( true == StringUtils.IsNotEmpty( txtMonthlySeqNum ) ) {
				txtMonthlySeqNum = getLargeTextChange( txtMonthlySeqNum );
			}

			//検索を実行する
			result.ListTable = AiImageCheckDao.SelectSearchAnlResultList( ddlLineCd, ddlStation, txtIdno, txtMonthlySeqNum, txtRecordNo, ngFlag, correctiveFlag, ref result );
			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 検査結果取得処理
		/// <summary>
		/// 検査結果取得処理：検索処理
		/// </summary>
		/// <param name="condition">検索条件</param>
		/// <param name="columnsDef">列定義</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <returns>処理結果</returns>
		public static ImgCheckResultSet SearchOfSearchAnlResult( Dictionary<string, object> condition, GridViewDefine[] columnsDef, int maxRecordCount ) {
			//変数定義
			var result = new ImgCheckResultSet();
			var cldStart = "";
			var cldEnd = "";

			//型式コードの大文字変換とハイフン削除
			if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MODEL_CD.bindField ) ) ) {
				condition[MainCorrectiveView.CONDITION.MODEL_CD.bindField] = getLargeTextChange( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MODEL_CD.bindField ) );
			}

			//月度連番の大文字変換とハイフン削除
			if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MONTHLYSEQNUM.bindField ) ) ) {
				condition[MainCorrectiveView.CONDITION.MONTHLYSEQNUM.bindField] = getLargeTextChange( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MONTHLYSEQNUM.bindField ) );
			}

			//型式名が空でないときは大文字にする
			if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MODEL_NM.bindField ) ) ) {
				condition[MainCorrectiveView.CONDITION.MODEL_NM.bindField] = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MODEL_NM.bindField ).ToUpper();
			}

			//機番が空でないときは大文字にする
			if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.KIBAN.bindField ) ) ) {
				condition[MainCorrectiveView.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.KIBAN.bindField ).ToUpper();
			}

			// 検査日開始
			if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.CLDSTART.bindField ) ) ) {
				cldStart = DataUtils.GetDictionaryDateVal( condition, MainCorrectiveView.CONDITION.CLDSTART.bindField ).ToShortDateString();
				cldStart = cldStart + " 00:00:00";
			}

			// 検査日終了
			if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.CLDEND.bindField ) ) ) {
				cldEnd = DataUtils.GetDictionaryDateVal( condition, MainCorrectiveView.CONDITION.CLDEND.bindField ).ToShortDateString();
				cldEnd = cldEnd + " 23:59:59";
			}

			// ライン
			var lineCd = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.LINE_CD.bindField );

			// ステーション
			var station = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.STATION.bindField );

			// 型式コード
			var modelCd = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MODEL_CD.bindField );

			// 型式名
			var modelNm = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MODEL_NM.bindField );

			// 機番
			var kiban = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.KIBAN.bindField );

			// IDNO
			var idno = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.IDNO.bindField );

			// 月度連番
			var monthlySeqNum = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.MONTHLYSEQNUM.bindField );

			// 検査結果
			var anlResult = DataUtils.GetDictionaryStringVal( condition, MainCorrectiveView.CONDITION.ANLRESULT.bindField );

			// 検索を実行する
			result.ListTable = AiImageCheckDao.SelectSearchResultList( lineCd, station, modelCd, modelNm, kiban, idno, monthlySeqNum, anlResult, cldStart, cldEnd, maxRecordCount, ref result );

			// 列定義から列名のキャプションを設定する
			columnsDef.ToList().ForEach( cd => {
				result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
			} );

			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 検査項目マスタ画面検索処理
		/// <summary>
		/// 検査項目マスタ画面：検索処理
		/// </summary>
		/// <param name="condition">検索条件</param>
		/// <param name="columnsDef">列定義</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <returns>処理結果</returns>
		public static ImgCheckResultSet SearchOfAnlItemResult( Dictionary<string, object> condition, GridViewDefine[] columnsDef, int maxRecordCount ) {
			//変数定義
			var result = new ImgCheckResultSet();

			// ライン
			var lineCd = DataUtils.GetDictionaryStringVal( condition, AnlItemView.CONDITION.LINE_CD.bindField );

			// ステーション
			var station = DataUtils.GetDictionaryStringVal( condition, AnlItemView.CONDITION.STATION.bindField );

			// 画像解析項目コード
			var itemCd = DataUtils.GetDictionaryStringVal( condition, AnlItemView.CONDITION.ITEM_CD.bindField );

			// 画像解析項目名
			var itemNm = DataUtils.GetDictionaryStringVal( condition, AnlItemView.CONDITION.ITEM_NM.bindField );

			// 利用ツール
			var usingTool = DataUtils.GetDictionaryStringVal( condition, AnlItemView.CONDITION.USING_TOOL.bindField );

			//利用ツールプルダウンタイプ
			var pulldownType = PULLDOWNTYPE_USINGTOOL;

			// 検索を実行する
			result.ListTable = AiImageCheckDao.SelectItemSearchList( lineCd, station, itemCd, itemNm, usingTool, pulldownType, maxRecordCount, ref result );

			// 列定義から列名のキャプションを設定する
			columnsDef.ToList().ForEach( cd => {
				result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
			} );

			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 画像解析グループ検索処理
		/// <summary>
		/// 画像解析グループ画面：検索処理
		/// </summary>
		/// <param name="condition">検索条件</param>
		/// <param name="columnsDef">列定義</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <returns>処理結果</returns>
		public static ImgCheckResultSet SelectGroupList( Dictionary<string, object> condition, GridViewDefine[] columnsDef, int maxRecordCount ) {
			//変数定義
			var result = new ImgCheckResultSet();

			// ステーション
			var station = DataUtils.GetDictionaryStringVal( condition, AnlGroupView.CONDITION.STATION.bindField );

			// ライン
			var lineCd = DataUtils.GetDictionaryStringVal( condition, AnlGroupView.CONDITION.LINE_CD.bindField );

			// 設備
			var setubi = DataUtils.GetDictionaryStringVal( condition, AnlGroupView.CONDITION.SETUBI.bindField );

			// 画像解析グループコード
			var imgAnlGroupCd = DataUtils.GetDictionaryStringVal( condition, AnlGroupView.CONDITION.IMG_GROUP_CD.bindField );

			// 画像解析グループ名
			var imgAnlGroupNm = DataUtils.GetDictionaryStringVal( condition, AnlGroupView.CONDITION.IMG_GROUP_NM.bindField );

			// 検索を実行する
			result.ListTable = AiImageCheckDao.SelectGroupList( station, lineCd, setubi, imgAnlGroupCd, imgAnlGroupNm, maxRecordCount, ref result );

			// 列定義から列名のキャプションを設定する
			columnsDef.ToList().ForEach( cd => {
				result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
			} );

			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 画像解析グループ入力検索処理
		/// <summary>
		/// 画像解析グループ入力画面：検索処理
		/// </summary>
		/// <param name="condition">検索条件</param>
		/// <param name="columnsDef">列定義</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <returns>処理結果</returns>
		public static ImgCheckResultSet SelectGroupInputList( Dictionary<string, object> condition, GridViewDefine[] columnsDef ) {
			//変数定義
			var result = new ImgCheckResultSet();

			// ステーション
			var station = DataUtils.GetDictionaryStringVal( condition, AnlGroupInput.GRID_CONDITION.STATION.bindField );

			// ライン
			var lineCd = DataUtils.GetDictionaryStringVal( condition, AnlGroupInput.GRID_CONDITION.LINE_CD.bindField );

			// 設備
			var setubi = DataUtils.GetDictionaryStringVal( condition, AnlGroupInput.GRID_CONDITION.SETUBI.bindField );

			// 画像解析グループコード
			var imgAnlGroupCd = DataUtils.GetDictionaryStringVal( condition, AnlGroupInput.GRID_CONDITION.IMG_GROUP_CD.bindField );

			// 検索を実行する
			result.ListTable = AiImageCheckDao.SelectGroupInputList( station, lineCd, setubi, imgAnlGroupCd, ref result );

			// 列定義から列名のキャプションを設定する
			columnsDef.ToList().ForEach( cd => {
				result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
			} );

			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region MS_SETUBI_JYOHO検索処理
		/// <summary>
		/// 型式紐づけ一覧画面：検索処理
		/// </summary>
		/// <param name="condition">検索条件</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <returns>処理結果</returns>
		public static ImgCheckResultSet SelectSetubiJyohoList( Dictionary<string, string> condition, int maxRecordCount ) {
			//変数定義
			var result = new ImgCheckResultSet();

			// ステーション
			var station = condition["txtStationNm"].ToString().Split( ':' )[0];

			// ライン
			var lineCd = condition["txtLineNm"].ToString().Split( ':' )[0];

			// 設備
			var setubi = condition["txtSetubi"].ToString();

			// 画像解析グループコード
			var imgAnlGroupCd = condition["txtGroupCd"].ToString();

			// 検索を実行する
			result.ListTable = AiImageCheckDao.SelectSetubiJyohoList( station, lineCd, setubi, imgAnlGroupCd, maxRecordCount, ref result );

			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 型式一覧取得処理
		/// <summary>
		/// 型式一覧取得処理
		/// </summary>
		/// <param name="txtGroupCd">グループコード</param>
		/// <param name="txtGroupNm">グループ名</param>
		/// <param name="txtModelCd">型式コード</param>
		/// <param name="txtModelNm">型式名</param>
		/// <param name="check">未割当</param>
		/// <param name="txtGroupCdNotExists">除外グループコード</param>
		/// <param name="txtStation">ステーションコード</param>
		public static ImgCheckResultSet SelectModelList( string txtGroupCd, string txtGroupNm, string txtModelCd, string txtModelNm, bool check, string txtGroupCdNotExists, string txtStation, int maxRecordCount ) {
			//変数定義
			var result = new ImgCheckResultSet();
			string checkValue = string.Empty;
			//型式コードの大文字変換とハイフン削除
			if ( true == StringUtils.IsNotEmpty( txtModelCd ) ) {
				txtModelCd = getLargeTextChange( txtModelCd );
			}

			if ( true == check ) {
				checkValue = "check";
			}
			//検索を実行する
			result.ListTable = AiImageCheckDao.SelectModelList( txtGroupCd, txtGroupNm, txtModelCd, txtModelNm, checkValue, txtGroupCdNotExists, txtStation, maxRecordCount, ref result );
			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 画像解析項目構成一覧検索処理 
		/// <summary>
		/// 画像解析項目構成一覧：検索処理
		/// </summary>
		/// <param name="condition">検索条件</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <returns>処理結果</returns>
		public static ImgCheckResultSet SelectGroupAsyList( Dictionary<string, string> condition, int maxRecordCount ) {
			//変数定義
			var result = new ImgCheckResultSet();

			// 画像解析グループコード
			var txtGroupCd = condition["txtGroupCd"].ToString();

			// 利用ツールプルダウンタイプ
			var pulldownType = PULLDOWNTYPE_USINGTOOL;

			// 検索を実行する
			result.ListTable = AiImageCheckDao.SelectGroupAsyList( txtGroupCd, pulldownType, maxRecordCount, ref result );


			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion

		#region 画像解析項目構成検索処理
		/// <summary>
		/// 画像解析項目構成：検索処理
		/// </summary>
		/// <param name="condition">検索条件</param>
		/// <param name="lineCd">ラインコード</param>
		/// <param name="stationCd">ステーションコード</param>
		/// <param name="groupCd">画像解析グループコード</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <returns>処理結果</returns>
		public static ImgCheckResultSet SelectGroupAsyJyoho( Dictionary<string, object> condition, string lineCd, string stationCd, string groupCd, int maxRecordCount ) {
			//変数定義
			var result = new ImgCheckResultSet();

			// 画像解析項目コード
			var itemCd = DataUtils.GetDictionaryStringVal( condition, AnlGroupListInput.CONDITION.ITEM_CD.bindField );

			// 画像解析項目名
			var itemNm = DataUtils.GetDictionaryStringVal( condition, AnlGroupListInput.CONDITION.ITEM_NM.bindField );

			// 利用ツール
			var usingTool = DataUtils.GetDictionaryStringVal( condition, AnlGroupListInput.CONDITION.USING_TOOL.bindField );

			// 利用ツールプルダウンタイプ
			var pulldownType = PULLDOWNTYPE_USINGTOOL;

			// 検索を実行する
			result.ListTable = AiImageCheckDao.SelectGroupAsyJyoho( itemCd, itemNm, usingTool, pulldownType, lineCd, stationCd, groupCd, maxRecordCount, ref result );


			if ( result.ListTable.Rows.Count == 0 ) {
				// 検索結果が0件の場合、エラーメッセージを設定
				result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
			}
			return result;
		}
		#endregion
	}
}