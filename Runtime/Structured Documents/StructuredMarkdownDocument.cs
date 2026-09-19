
#if MARKDIG
namespace TeaSpoons.StructuredDocuments
{
    using Markdig;
    using Markdig.Extensions.Tables;
    using Markdig.Syntax;
    using Markdig.Syntax.Inlines;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A <see cref="StructuredDocument"/> based on a markdown file.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>If the first block is a heading, its content is stored in the <see cref="Title"/> field and ignored for data paths.</item>
    /// </list>
    /// </remarks>
    public class StructuredMarkdownDocument : PreloadingStructuredDocument
    {
        private static class MarkdownBlockConverters
        {
            private static readonly Dictionary<Type, Func<StructuredMarkdownDocument, Block, DataBlock>> converters = new();

            static MarkdownBlockConverters()
            {
                AddConverter<ParagraphBlock, TextBlock>((document, block) =>
                {
                    return new TextBlock(document.GetSourceSubstring(block.Span));
                });

                AddConverter<QuoteBlock, TextBlock>((document, block) =>
                {
                    return new TextBlock(document.GetSourceSubstring(block.Span).Substring(1).Trim());
                });

                AddConverter<Markdig.Syntax.ListBlock, ListBlock>((document, block) =>
                {
                    var list = new List<string>();
                    foreach (var itemBlock in block.Cast<ListItemBlock>())
                    {
                        var contentBlock = (ParagraphBlock)itemBlock[0];

                        var line = document.GetSourceSubstring(contentBlock.Span);
                        list.Add(line);
                    }
                    return new ListBlock(list, TrySplitAtColon);
                });

                AddConverter<Table, TableBlock>((document, block) =>
                {
                    var tableBlock = new TableBlock();

                    var resultRows = new List<List<string>>();
                    foreach (var row in block.Cast<TableRow>())
                    {
                        if (row.IsHeader)
                        {
                            var columnKeys = new List<string>();
                            foreach (var cell in row.Cast<TableCell>())
                            {
                                var cellContent = document.GetSourceSubstring(cell.Span).Trim().ToLowerInvariant();
                                columnKeys.Add(cellContent);
                            }
                            tableBlock.SetColumnKeys(columnKeys);
                        }
                        else
                        {
                            var cells = new List<string>();
                            resultRows.Add(cells);

                            foreach (var cell in row.Cast<TableCell>())
                            {
                                var cellContent = document.GetSourceSubstring(cell.Span).Trim();
                                cells.Add(cellContent);
                            }

                            tableBlock.AddRow(cells);
                        }
                    }

                    return tableBlock;
                });
            }

            private static void AddConverter<TInput, TOutput>(Func<StructuredMarkdownDocument, TInput, TOutput> converter)
                where TInput : Block
                where TOutput : DataBlock
            {
                converters.Add(typeof(TInput), (document, block) => converter(document, (TInput)block));
            }

            public static bool TryConvert(StructuredMarkdownDocument document, Block block, out DataBlock dataBlock)
            {
                if (converters.TryGetValue(block.GetType(), out var converter))
                {
                    dataBlock = converter(document, block);
                    return true;
                }

                dataBlock = null;
                return false;
            }
        }

        private readonly HeadingPath headingPathBuffer = new();

        private string source;
        public MarkdownDocument Document { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public override void Clear()
        {
            base.Clear();

            headingPathBuffer.Clear();
            source = null;
            Document = null;
            Title = string.Empty;
        }

        protected override void OnLoad(string source)
        {
            var pipeline = GetMarkdownPipeline();

            Document = Markdown.Parse(source, pipeline);
            this.source = source;

            var subBlockIndex = 0;

            var firstBlock = true;

            foreach (var block in Document)
            {
                if (block is HeadingBlock heading)
                {
                    var name = GetInlineText(heading.Inline);

                    if (firstBlock)
                    {
                        Title = name;
                    }
                    else
                    {
                        var level = (byte)heading.Level;
                        headingPathBuffer.UpdateHeader(level, name.ToLowerInvariant());

                        subBlockIndex = 0;
                    }
                }
                else
                {
                    if (MarkdownBlockConverters.TryConvert(this, block, out var dataBlock))
                    {
                        var path = headingPathBuffer.ToPath();
                        if (data.GetValueCount(path) != subBlockIndex)
                        {
                            // TODO Create custom exception class
                            throw new Exception("Collision between equal heading paths.");
                        }

                        data.Add(path, dataBlock);

                        subBlockIndex++;
                    }
                }

                firstBlock = false;
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="MarkdownPipeline"/> that has table parsing enabled.
        /// </summary>
        private static MarkdownPipeline GetMarkdownPipeline()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.UsePipeTables(new PipeTableOptions
            {
                RequireHeaderSeparator = false
            });
            var pipeline = pipelineBuilder.Build();
            return pipeline;
        }

        private string GetSourceSubstring(SourceSpan span)
        {
            return source.Substring(span.Start, span.Length);
        }

        private string GetInlineText(ContainerInline inline)
        {
            var result = new StringBuilder();
            var current = inline.FirstChild;
            
            while (current != null)
            {
                if (current is LiteralInline literal)
                {
                    result.Append(literal.Content.ToString());
                }
                else if (current is ContainerInline container)
                {
                    result.Append(GetInlineText(container));
                }
                else
                {
                    result.Append(GetSourceSubstring(current.Span));
                }
                
                current = current.NextSibling;
            }
            
            return result.ToString();
        }

        private static bool TrySplitAtColon(string line, out string key, out string value)
        {
            var colonIndex = line.IndexOf(':');
            if (colonIndex > -1)
            {
                key = line.Substring(0, colonIndex).Trim().ToLowerInvariant();
                value = line.Substring(colonIndex + 1).Trim();
                return true;
            }

            key = null;
            value = null;
            return false;
        }
    }
}
#endif
